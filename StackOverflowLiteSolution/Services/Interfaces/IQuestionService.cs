using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Strategies.Interfaces;

namespace Stackoverflow_Lite.Services.Interfaces;

public interface IQuestionService
{
    Task<IEnumerable<QuestionDto>> GetQuestionsAsync(int offset, int size);
    Task<IEnumerable<QuestionDto>> GetFilteredQuestionsAsync(IEnumerable<IQuestionFilterStrategy> strategies);

    Task<Guid> CreateQuestionAsync(string token, QuestionRequest questionRequest);
    Task<Question> GetQuestionAsync(string token, Guid questionId, bool removeFilter = false);

    Task DeleteQuestionAsync(string token,Guid questionId);
    Task DeleteQuestionAdminAsync(Guid questionId);

    Task<QuestionStatusDto> IsQuestionProcessedAsync(string token, Guid questionId);

    Task<Question> EditQuestionAsync(string token, Guid questionId, QuestionRequest questionRequest);
    
    Task<Question> VoteQuestionAsync(QuestionVoteRequest voteRequest);

}