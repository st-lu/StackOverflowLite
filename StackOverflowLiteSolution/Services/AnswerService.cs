using Stackoverflow_Lite.BackgroundTasks;
using Stackoverflow_Lite.Repositories;
using Stackoverflow_Lite.Services;
using Stackoverflow_Lite.Configurations;
using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Exceptions;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Repositories;
using Stackoverflow_Lite.Services.Interfaces;
using Stackoverflow_Lite.Utils;
using Stackoverflow_Lite.Utils.Interfaces;

namespace Stackoverflow_Lite.Services;

public class AnswerService(
    IAnswerRepository answerRepository,
    ITokenClaimsExtractor tokenClaimsExtractor,
    IUserService userService,
    IBackgroundTaskScheduler backgroundTaskScheduler,
    IServiceScopeFactory serviceScopeFactory,
    IInputAnalyzer inputAnalyzer,
    IEmailService emailService) : IAnswerService
{


    public async Task<Guid> CreateAnswerAsync(string token, Guid questionId , AnswerRequest answerRequest)
    {
        var subClaim = tokenClaimsExtractor.ExtractClaim(token, "sub");
        var userId = await userService.GetUserIdFromSubClaimAsync(subClaim);
        var answer = new Answer
        {
            Content = answerRequest.Content,
            UserId = userId,
            QuestionId = questionId
        };
        var createdAnswer = await answerRepository.CreateAnswerAsync(answer);
        
        await backgroundTaskScheduler.QueueBackgroundWorkItemAsync(async token =>
        {
            var result = await inputAnalyzer.Analyze(answer.Content, token);
            using var scope = serviceScopeFactory.CreateScope();
            var answerRepository = scope.ServiceProvider.GetRequiredService<IAnswerRepository>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var user = await userRepository.GetUserAsync(userId);
            await answerRepository.UpdateAnswerTextCategoryAsync(createdAnswer.Id, result);

            await emailService.SendEmailAsync(PostType.ANSWER, user.Email, answer.Content, result == TextCategory.Accepted, false);
        });

        return answer.Id;
    }
    public async Task DeleteAnswerAsync(string token,Guid answerId)
    {
        if (!await IsCurrentUserAnswerAuthor(token, answerId))
            throw new OperationNotAllowed(ApplicationConstants.OPERATION_NOT_ALLOWED_MESSAGE);
        await answerRepository.DeleteAnswerAsync(answerId);
    }
    public async Task<Answer> EditAnswerAsync(string token, Guid answerId, AnswerRequest answerRequest)
    {
        if (!await IsCurrentUserAnswerAuthor(token, answerId))
            throw new OperationNotAllowed(ApplicationConstants.OPERATION_NOT_ALLOWED_MESSAGE);
        return await answerRepository.EditAnswerAsync(answerId, answerRequest);
    }
    public async Task DeleteAnswerAdminAsync(Guid answerId)
    {
        await answerRepository.DeleteAnswerAsync(answerId);
    }
    private async Task<bool> IsCurrentUserAnswerAuthor(string token, Guid answerId)
    {
        var subClaim = tokenClaimsExtractor.ExtractClaim(token,"sub");
        var userId = await userService.GetUserIdFromSubClaimAsync(subClaim);
        return userId == await answerRepository.GetAuthorIdFromAnswerIdAsync(answerId);
    }
}