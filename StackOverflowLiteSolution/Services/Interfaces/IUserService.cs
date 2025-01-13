using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.models;

namespace Stackoverflow_Lite.Services.Interfaces;

public interface IUserService
{
    Task<User> CreateMappingAsync(string token);
    Task<Guid> GetUserIdFromSubClaimAsync(string subClaim);

    Task<List<Question>> GetAllUserQuestions(string token);

    Task<User> GetUserAsync(string token);

    Task<List<UserDto>> GetMostActiveUsers(string token);
}