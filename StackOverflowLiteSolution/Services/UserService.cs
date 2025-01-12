using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Exceptions;
using Stackoverflow_Lite.Services;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Repositories;
using Stackoverflow_Lite.Services.Interfaces;
using Stackoverflow_Lite.Utils;
using Stackoverflow_Lite.Utils.Interfaces;

namespace Stackoverflow_Lite.services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenClaimsExtractor _tokenClaimsExtractor;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ITokenClaimsExtractor tokenClaimsExtractor, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _tokenClaimsExtractor = tokenClaimsExtractor;
        _logger = logger;
    }

    public async Task<User> CreateMappingAsync(string token)
    {
        var subClaim = _tokenClaimsExtractor.ExtractClaim(token, "sub");
        var username = _tokenClaimsExtractor.ExtractClaim(token, "preferred_username");
        if (subClaim == string.Empty || username == string.Empty)
        {
            throw new ArgumentException(String.Format(ApplicationConstants.OIDC_CLAIMS_EXTRACTION_ERROR, subClaim, username));
        }
        // check if the user mapping was not previously created ( to avoid created multiple user DB instances for the same Keycloak user)
        try
        {
            await GetUserIdFromSubClaimAsync(subClaim);
        }
        catch (OidcUserMappingNotFound)
        {
            var user = new User
            {
                Username = username,
                OidcUserMapping = new OidcUserMapping { SubClaim = subClaim }
            };
            return await _userRepository.CreateUserAsync(user);
        }

        throw new OidcUserMappingAlreadyCreated(ApplicationConstants.OIDC_MAPPING_ALREADY_CREATED);

    }

    public async Task<Guid> GetUserIdFromSubClaimAsync(string subClaim)
    {
        var oidcUserMapping = await _userRepository.GetOidcUserMappingFromSubClaimAsync(subClaim);
        return oidcUserMapping.UserId;
    }

    public async Task<List<Question>> GetAllUserQuestions(string token)
    {
        var subClaim = _tokenClaimsExtractor.ExtractClaim(token, "sub");
        var userId = await GetUserIdFromSubClaimAsync(subClaim);

        var questions = await _userRepository.GetAllUserQuestions(userId);

        return questions;
    }
    
}