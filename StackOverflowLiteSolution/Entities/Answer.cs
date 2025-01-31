using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Stackoverflow_Lite.models;

namespace Stackoverflow_Lite.Entities;

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
    
    public bool IsVisible { get; set; } = false;

    public TextCategory TextCategory { get; set; }

}