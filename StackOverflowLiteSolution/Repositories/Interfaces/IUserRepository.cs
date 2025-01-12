using Stackoverflow_Lite.Entities;

namespace Stackoverflow_Lite.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);
    Task<OidcUserMapping> GetOidcUserMappingFromSubClaimAsync(string subClaim);
    Task<List<Question>> GetAllUserQuestions(Guid userId);

}
