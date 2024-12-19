using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Stackoverflow_Lite.Entities;

namespace Stackoverflow_Lite.Entities;

public class UserQuestionView
{

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid QuestionId { get; set; }

    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public Question Question { get; set; }
}