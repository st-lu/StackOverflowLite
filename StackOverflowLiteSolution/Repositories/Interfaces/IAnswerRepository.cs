using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.models;

namespace Stackoverflow_Lite.Repositories;

public interface IAnswerRepository
{
    Task<Answer> CreateAnswerAsync(Answer answer);
    Task<Answer> EditAnswerAsync(Guid answerId,AnswerRequest answerRequest);

    Task DeleteAnswerAsync(Guid answerId);
    Task<Guid> GetAuthorIdFromAnswerIdAsync(Guid questionId);
    
    Task UpdateAnswerTextCategoryAsync(Guid answerId, TextCategory textCategory);
    
    Task<Answer> VoteAnswerAsync(Guid answerId, int value);



}