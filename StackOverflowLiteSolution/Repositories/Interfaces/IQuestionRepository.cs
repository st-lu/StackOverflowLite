using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.models;

namespace Stackoverflow_Lite.Repositories;

public interface IQuestionRepository
{
    Task<Question> CreateQuestionAsync(Question question);
    Task<Question?> GetQuestionAsync(Guid questionId);
    Task<Guid> GetAuthorIdFromQuestionIdAsync(Guid questionId);

    Task DeleteQuestionAsync(Guid questionId);

    Task<Question> EditQuestionAsync(Guid questionId, QuestionRequest questionRequest);
    Task<List<Question>> GetQuestionsBatchAsync(int offset, int size);

    Task TryToIncrementViewQuestionCount(Question question, Guid userId);

}