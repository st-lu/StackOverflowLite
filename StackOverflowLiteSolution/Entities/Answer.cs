using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Stackoverflow_Lite.Entities;

[Table("Answer")]
public class Answer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Content { get; set; }
    // variable to hold the ratio between upvotes and downvotes 
    public int Score { get; set; } = 0;
    // Foreign key to User
    public Guid UserId { get; set; }
    // Foreign key to Question
    public Guid QuestionId { get; set; }
    // Navigation property
    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore] 
    public Question Question;

}