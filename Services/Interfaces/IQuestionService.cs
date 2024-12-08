using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.models;

namespace Stackoverflow_Lite.Services.Interfaces;

public interface IQuestionService
{
    Task<IEnumerable<QuestionDto>> GetQuestionsAsync(int offset, int size);
    Task<Question> CreateQuestionAsync(string token, QuestionRequest questionRequest);
    Task<Question> GetQuestionAsync(string token, Guid questionId);

    Task DeleteQuestionAsync(string token,Guid questionId);
    Task DeleteQuestionAdminAsync(Guid questionId);

    Task<Question> EditQuestionAsync(string token, Guid questionId, QuestionRequest questionRequest);

}