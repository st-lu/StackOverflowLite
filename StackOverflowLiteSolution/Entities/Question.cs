using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.models;

namespace Stackoverflow_Lite.Entities;

public class Question
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required]
    public string Content { get; set; }
    // variable to hold the ratio between upvotes and downvotes 
    public int Score { get; set; } = 0;
    // Foreign key to User
    public Guid UserId { get; set; }

    // Navigation property
    [JsonIgnore]
    public User User { get; set; }

    public bool IsVisible { get; set; } = false;

    public TextCategory TextCategory { get; set; } = TextCategory.Processing;

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
     
    // variable that holds the number of UNIQUE viewers of a question 
    // can also be computed by running a SQL count on the associative table between User and Question entities
    // stored directly in the Question table + updated regularly for fast question ordering on demand 
    public int ViewsCount { get; set; } = 0;
    [JsonIgnore]
    public ICollection<UserQuestionView> UserQuestionViews { get; set; } = new List<UserQuestionView>();

}