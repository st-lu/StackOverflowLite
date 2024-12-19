using Stackoverflow_Lite.models;

namespace Stackoverflow_Lite.Services.Interfaces
{
    public interface IInputAnalyzer
    {
        Task<TextCategory> Analyze(string text, CancellationToken cancellation);
    }
}
