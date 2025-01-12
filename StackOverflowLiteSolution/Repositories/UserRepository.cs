using Microsoft.EntityFrameworkCore;
using Stackoverflow_Lite.Configurations;
using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Exceptions;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Utils;

namespace Stackoverflow_Lite.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<OidcUserMapping> GetOidcUserMappingFromSubClaimAsync(string subClaim)
    {
        var oidcUserMapping = await _context.OidcUserMappings.FirstOrDefaultAsync(o => o.SubClaim == subClaim);
        if (oidcUserMapping == null)
            throw new OidcUserMappingNotFound(ApplicationConstants.OIDC_MAPPING_NOT_CREATED);

        return oidcUserMapping;
    }
    
    public async Task<List<Question>> GetAllUserQuestions(Guid userId)
    {
        return await _context.Questions
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    
}
