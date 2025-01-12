using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.models;

namespace Stackoverflow_Lite.Services;

public interface IAnswerService
{
    Task<Guid> CreateAnswerAsync(string token, Guid questionId , AnswerRequest answerRequest);
    Task DeleteAnswerAsync(string token, Guid answerId);
    Task<Answer> EditAnswerAsync(string token, Guid tokenId, AnswerRequest answerRequest);
    Task DeleteAnswerAdminAsync(Guid answerId);
    Task<Answer> VoteAnswerAsync(AnswerVoteRequest voteRequest);


}