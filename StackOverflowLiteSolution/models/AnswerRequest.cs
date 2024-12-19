using System.ComponentModel.DataAnnotations;

namespace Stackoverflow_Lite.models;

public class AnswerRequest
{
    [Required]
    [StringLength(200, MinimumLength = 5)]
    public string Content { get; set; }
}