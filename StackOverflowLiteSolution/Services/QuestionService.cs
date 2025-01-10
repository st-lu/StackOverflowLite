using Microsoft.Extensions.Caching.Memory;
using Stackoverflow_Lite.Repositories;
using Stackoverflow_Lite.Configurations;
using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Exceptions;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Repositories;
using Stackoverflow_Lite.Services.Interfaces;
using Stackoverflow_Lite.Utils;
using Stackoverflow_Lite.Utils.Interfaces;
using Stackoverflow_Lite.BackgroundTasks;
using Microsoft.Extensions.DependencyInjection;
using Stackoverflow_Lite.Strategies.Interfaces;

namespace Stackoverflow_Lite.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly ITokenClaimsExtractor _tokenClaimsExtractor;
    private readonly IUserService _userService;
    private readonly IMemoryCache _memoryCache; 
    private readonly ILogger<QuestionService> _logger;    
    private readonly IInputAnalyzer _inputAnalyzer;
    private readonly IBackgroundTaskScheduler _backgroundTaskScheduler;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly int _batchSize;
    

    public QuestionService(IQuestionRepository questionRepository, ITokenClaimsExtractor tokenClaimsExtractor, IUserService userService, ILogger<QuestionService> logger, IMemoryCache memoryCache, IConfiguration configuration, IInputAnalyzer inputAnalyzer, IBackgroundTaskScheduler backgroundTaskScheduler, IServiceScopeFactory serviceScopeFactory)
    {
        _questionRepository = questionRepository;
        _tokenClaimsExtractor = tokenClaimsExtractor;
        _userService = userService;
        _logger = logger;
        _batchSize = configuration.GetValue<int>("DbBatchSize");
        _batchSize = 40;
        _memoryCache = memoryCache;
        _inputAnalyzer = inputAnalyzer;
        _backgroundTaskScheduler = backgroundTaskScheduler;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task<IEnumerable<QuestionDto>> GetQuestionsAsync(int offset, int size)
    {
        string cacheKey = $"questions_batch_{(offset + size) / _batchSize}";
        if (!_memoryCache.TryGetValue(cacheKey, out List<Question> cachedQuestions))
        {
            cachedQuestions = await _questionRepository.GetQuestionsBatchAsync(offset, _batchSize);
            var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10));

            _memoryCache.Set(cacheKey, cachedQuestions, cacheOptions);
        }

        var selectedQuestions = cachedQuestions.Skip(offset % _batchSize).Take(size);
    
        // Map questions to QuestionDto
        var questionDtos = selectedQuestions.Select(q => new QuestionDto
        {
            Score = q.Score,
            ViewCount = q.ViewsCount,
            Content = q.Content,
            AuthorId = q.UserId,
            QuestionId = q.Id
        });

        return questionDtos;    
    }
    
    public async Task<IEnumerable<QuestionDto>> GetFilteredQuestionsAsync(IEnumerable<IQuestionFilterStrategy> strategies)
    {
        var questions = await _questionRepository.GetAllQuestions();

        IEnumerable<Question> filteredQuestions = questions;
        
        foreach (var strategy in strategies)
        {
            filteredQuestions = strategy.ApplyFilter(filteredQuestions);
        }
        
        if (filteredQuestions.Count() == questions.Count() &&
            filteredQuestions.All(q => questions.Contains(q)))
        {
            return Enumerable.Empty<QuestionDto>();
        }

        // apply filtering strategies
        var questionDtos = filteredQuestions.Select(q => new QuestionDto
        {
            Score = q.Score,
            ViewCount = q.ViewsCount,
            Content = q.Content,
            AuthorId = q.UserId,
            QuestionId = q.Id
        });

        return questionDtos; 

    }

    
    public async Task<Guid> CreateQuestionAsync(string token, QuestionRequest questionRequest)
    {
        var subClaim = _tokenClaimsExtractor.ExtractClaim(token, "sub");
        var userId = _userService.GetUserIdFromSubClaimAsync(subClaim);
        var question = new Question
        {
            Content = questionRequest.Content,
            UserId = userId.Result
        };
        var createdQuestion = await _questionRepository.CreateQuestionAsync(question);

        await _backgroundTaskScheduler.QueueBackgroundWorkItemAsync(async token =>
        {
            var result = await _inputAnalyzer.Analyze(question.Content, token);
            using var scope = _serviceScopeFactory.CreateScope();
            var questionRepository = scope.ServiceProvider.GetRequiredService<IQuestionRepository>();
            await questionRepository.UpdateQuestionTextCategoryAsync(createdQuestion.Id, result);
        });
        return question.Id;
    }

    public async Task<Question> GetQuestionAsync(string token, Guid questionId)
    {
        var question =  await _questionRepository.GetQuestionAsync(questionId);
        var userId = await _userService.GetUserIdFromSubClaimAsync(_tokenClaimsExtractor.ExtractClaim(token, "sub"));
        await _questionRepository.TryToIncrementViewQuestionCount(question, userId);
        return question;
    }

    public async Task DeleteQuestionAsync(string token,Guid questionId)
    {
        if (!await IsCurrentUserQuestionAuthor(token, questionId))
            throw new OperationNotAllowed(ApplicationConstants.OPERATION_NOT_ALLOWED_MESSAGE);
        await _questionRepository.DeleteQuestionAsync(questionId);
    }

    public async Task DeleteQuestionAdminAsync(Guid questionId)
    {
        await _questionRepository.DeleteQuestionAsync(questionId);
    }

    public async Task<Question> EditQuestionAsync(string token, Guid questionId, QuestionRequest questionRequest)
    {
        if (!await IsCurrentUserQuestionAuthor(token, questionId))
            throw new OperationNotAllowed(ApplicationConstants.OPERATION_NOT_ALLOWED_MESSAGE);
        return await _questionRepository.EditQuestionAsync(questionId, questionRequest);
    }

    private async Task<bool> IsCurrentUserQuestionAuthor(string token, Guid questionId)
    {
        var subClaim = _tokenClaimsExtractor.ExtractClaim(token,"sub");
        var userId = await _userService.GetUserIdFromSubClaimAsync(subClaim);
        return userId == await _questionRepository.GetAuthorIdFromQuestionIdAsync(questionId);
    }


}