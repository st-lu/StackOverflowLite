using Stackoverflow_Lite.Entities;

namespace Stackoverflow_Lite.Strategies.Interfaces;

public interface IQuestionFilterStrategy
{
    IEnumerable<Question> ApplyFilter(IEnumerable<Question> questions);
}
