using System.ComponentModel.DataAnnotations;
using Stackoverflow_Lite.Entities;

namespace Stackoverflow_Lite.models;

public class AnswerDto
{
    public string Content { get; set; }
    public User User { get; set; }
    public int Score { get; set; }
    public Guid AuthorId { get; set; }
}