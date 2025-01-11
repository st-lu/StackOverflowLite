using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Strategies.Interfaces;

namespace Stackoverflow_Lite.Strategies;

public class TextFilterStrategy : IQuestionFilterStrategy
{
    private readonly string _searchText;

    public TextFilterStrategy(string searchText)
    {
        _searchText = searchText;
    }

    public IEnumerable<Question> ApplyFilter(IEnumerable<Question> questions)
    {
        return questions.Where(q => q.Content.Contains(_searchText));
    }
}
