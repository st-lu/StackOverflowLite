using Stackoverflow_Lite.Entities;

namespace Stackoverflow_Lite.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);

    Task<User> GetUserAsync(Guid userId);

    Task<OidcUserMapping> GetOidcUserMappingFromSubClaimAsync(string subClaim);
}
