namespace Stackoverflow_Lite.models;

public class QuestionDto
{
    // variable to hold the ratio between upvotes and downvotes 
    public Guid QuestionId { get; set; }
    public int Score { get; set; }
    public int ViewCount { get; set; }
    public string Content { get; set; }
    public Guid AuthorId { get; set; }
}