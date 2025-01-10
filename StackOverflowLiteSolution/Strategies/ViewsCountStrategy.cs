using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Strategies.Interfaces;

namespace Stackoverflow_Lite.Strategies;

public class ViewsCountStrategy : IQuestionFilterStrategy
{
    private readonly bool _ascending;
    
    public ViewsCountStrategy(bool ascending = false)
    {
        _ascending = ascending;
    }
    
    public IEnumerable<Question> ApplyFilter(IEnumerable<Question> questions)
    {
        return _ascending 
            ? questions.OrderBy(q => q.ViewsCount) 
            : questions.OrderByDescending(q => q.ViewsCount);
    }
    
}