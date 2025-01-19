namespace Stackoverflow_Lite.models
{
    public class QuestionStatusDto(bool processed)
    {
        public bool Processed { get; } = processed;
    }
}
