namespace Stackoverflow_Lite.models;

public class UserDto
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public int NumberQuestions { get; set; }
    
    public int NumberAnswers{ get; set; }
    
    public int Score{ get; set; }

}