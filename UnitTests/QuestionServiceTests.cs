using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Stackoverflow_Lite.Repositories;
using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Exceptions;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Services.Interfaces;
using Stackoverflow_Lite.Services;
using Stackoverflow_Lite.Utils.Interfaces;
using Stackoverflow_Lite.Utils;
using NSubstitute.ExceptionExtensions;

namespace UnitTests;

[TestFixture]
public class QuestionServiceTests
{
    private IQuestionRepository _questionRepository;
    private ITokenClaimsExtractor _tokenClaimsExtractor;
    private IUserService _userService;
    private IMemoryCache _memoryCache;
    private ILogger<QuestionService> _logger;
    private QuestionService _questionService;
    private readonly int _batchSize = 40;

    [TearDown]
    public void TearDown()
    {
        if (_memoryCache is IDisposable disposableCache)
        {
            disposableCache.Dispose();
        }
    }

    [SetUp]
    public void Setup()
    {
        _questionRepository = Substitute.For<IQuestionRepository>();
        _tokenClaimsExtractor = Substitute.For<ITokenClaimsExtractor>();
        _userService = Substitute.For<IUserService>();
        _memoryCache = Substitute.For<IMemoryCache>();
        _logger = Substitute.For<ILogger<QuestionService>>();
        // using real instance of in-memory configuration builder to avoid errors when mocking
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                    new KeyValuePair<string, string>("DbBatchSize", _batchSize.ToString())
            })
            .Build();

        _questionService = new QuestionService(_questionRepository, _tokenClaimsExtractor, _userService, _logger, _memoryCache, configuration);
    }

    [Test]
    public async Task GetQuestionsAsync_ShouldReturnQuestions_WhenCacheMiss()
    {
        var offset = 0;
        var size = 5;
        var questions = new List<Question>
            {
                new Question { Content = "Question 1" },
                new Question { Content = "Question 2" },
                new Question { Content = "Question 3" },
                new Question { Content = "Question 4" },
                new Question { Content = "Question 5" }
            };
        _questionRepository.GetQuestionsBatchAsync(offset, _batchSize).Returns(questions);
        // simulate cache miss
        _memoryCache.TryGetValue(Arg.Any<string>(), out Arg.Any<List<Question>>()).Returns(false);

        var result = await _questionService.GetQuestionsAsync(offset, size);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(size));
    }

    [Test]
    public async Task GetQuestionsAsync_ShouldReturnCachedQuestions_WhenCacheHit()
    {
        var offset = 0;
        var size = 5;
        var questions = new List<Question>
            {
                new Question { Content = "Question 1" },
                new Question { Content = "Question 2" }
            };
        //simulate cache hit
        _memoryCache.TryGetValue(Arg.Any<string>(), out var cachedQuestions).Returns(x =>
        {
            x[1] = questions;
            return true;
        });

        var result = await _questionService.GetQuestionsAsync(offset, size);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(questions.Count));
    }

    [Test]
    [Ignore("")]
    public async Task CreateQuestionAsync_ShouldReturnQuestion_WhenValidRequest()
    {
        var token = "valid-token";
        var questionRequest = new QuestionRequest { Content = "Test Question" };
        var userId = Guid.NewGuid();
        var question = new Question
        {
            Content = questionRequest.Content,
            UserId = userId
        };

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(userId));
        _questionRepository.CreateQuestionAsync(Arg.Any<Question>()).Returns(Task.FromResult(question));

        await _questionService.CreateQuestionAsync(token, questionRequest);

        //Assert.That(result, Is.Not.Null);
        //Assert.That(result.Content, Is.EqualTo(questionRequest.Content));
        //Assert.That(result.UserId, Is.EqualTo(userId));
    }


    [Test]
    public async Task GetQuestionAsync_ShouldReturnQuestion_WhenValidRequest()
    {
        var token = "valid-token";
        var questionId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var question = new Question { Id = questionId };

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(userId));
        _questionRepository.GetQuestionAsync(questionId).Returns(Task.FromResult(question));
        _questionRepository.TryToIncrementViewQuestionCount(question, userId).Returns(Task.CompletedTask);

        var result = await _questionService.GetQuestionAsync(token, questionId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(questionId));
    }

    [Test]
    public async Task GetQuestionAsync_ShouldThrowException_WhenQuestionNotFound()
    {
        var token = "valid-token";
        var questionId = Guid.NewGuid();

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(Guid.NewGuid()));
        _questionRepository.GetQuestionAsync(questionId).ThrowsAsync(new EntityNotFound(ApplicationConstants.QUESTION_NOT_FOUND_MESSAGE));

        Assert.ThrowsAsync<EntityNotFound>(async () => await _questionService.GetQuestionAsync(token, questionId));

    }

    [Test]
    public async Task DeleteQuestionAsync_ShouldDeleteQuestion_WhenUserIsAuthor()
    {
        var token = "valid-token";
        var questionId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(userId));
        _questionRepository.GetAuthorIdFromQuestionIdAsync(questionId).Returns(Task.FromResult(userId));

        await _questionService.DeleteQuestionAsync(token, questionId);

        await _questionRepository.Received().DeleteQuestionAsync(questionId);
    }

    [Test]
    public async Task DeleteQuestionAsync_ShouldThrowOperationNotAllowed_WhenUserIsNotAuthor()
    {
        var token = "valid-token";
        var questionId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(userId));
        _questionRepository.GetAuthorIdFromQuestionIdAsync(questionId).Returns(Task.FromResult(Guid.NewGuid())); // Different userId

        var ex = Assert.ThrowsAsync<OperationNotAllowed>(async () => await _questionService.DeleteQuestionAsync(token, questionId));
        Assert.That(ex.Message, Is.EqualTo(ApplicationConstants.OPERATION_NOT_ALLOWED_MESSAGE));
    }

    [Test]
    public async Task DeleteQuestionAdminAsync_ShouldDeleteQuestion()
    {
        var questionId = Guid.NewGuid();

        await _questionService.DeleteQuestionAdminAsync(questionId);

        await _questionRepository.Received().DeleteQuestionAsync(questionId);
    }

    [Test]
    public async Task EditQuestionAsync_ShouldReturnUpdatedQuestion_WhenUserIsAuthor()
    {
        var token = "valid-token";
        var questionId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var questionRequest = new QuestionRequest { Content = "Updated Content" };
        var updatedQuestion = new Question
        {
            Content = questionRequest.Content,
            UserId = userId
        };

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(userId));
        _questionRepository.GetAuthorIdFromQuestionIdAsync(questionId).Returns(Task.FromResult(userId));
        _questionRepository.EditQuestionAsync(questionId, questionRequest).Returns(Task.FromResult(updatedQuestion));

        var result = await _questionService.EditQuestionAsync(token, questionId, questionRequest);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Content, Is.EqualTo(questionRequest.Content));
    }

    [Test]
    public void EditQuestionAsync_ShouldThrowOperationNotAllowed_WhenUserIsNotAuthor()
    {
        var token = "valid-token";
        var questionId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var questionRequest = new QuestionRequest { Content = "Updated Content" };

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(userId));
        _questionRepository.GetAuthorIdFromQuestionIdAsync(questionId).Returns(Task.FromResult(Guid.NewGuid())); // Different userId

        var ex = Assert.ThrowsAsync<OperationNotAllowed>(async () => await _questionService.EditQuestionAsync(token, questionId, questionRequest));
        Assert.That(ex.Message, Is.EqualTo(ApplicationConstants.OPERATION_NOT_ALLOWED_MESSAGE));
    }
}