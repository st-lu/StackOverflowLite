using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.models;

namespace Stackoverflow_Lite.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);

    Task<User> GetUserAsync(Guid userId);

    Task<OidcUserMapping> GetOidcUserMappingFromSubClaimAsync(string subClaim);
    Task<List<Question>> GetAllUserQuestions(Guid userId);

    Task<List<UserDto>> GetMostActiveUsers();

}
