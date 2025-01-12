using Stackoverflow_Lite.Entities;

namespace Stackoverflow_Lite.Services.Interfaces;

public interface IUserService
{
    Task<User> CreateMappingAsync(string token);
    Task<Guid> GetUserIdFromSubClaimAsync(string subClaim);

    Task<List<Question>> GetAllUserQuestions(string token);
}