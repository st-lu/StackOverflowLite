using Microsoft.Extensions.Logging;
using NSubstitute;
using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Exceptions;
using Stackoverflow_Lite.Repositories;
using Stackoverflow_Lite.services;
using Stackoverflow_Lite.Utils.Interfaces;
using Stackoverflow_Lite.Utils;

namespace UnitTests;
[TestFixture]
public class UserServiceTests
{
    private IUserRepository _userRepositorySub;
    private ITokenClaimsExtractor _tokenClaimsExtractorSub;
    private UserService _userService;
    private ILogger<UserService> _logger;


    [SetUp]
    public void Setup()
    {
        _userRepositorySub = Substitute.For<IUserRepository>();
        _tokenClaimsExtractorSub = Substitute.For<ITokenClaimsExtractor>();
        _logger = Substitute.For<ILogger<UserService>>();
        _userService = new UserService(_userRepositorySub, _tokenClaimsExtractorSub, _logger);
    }

    [Test]
    [Ignore("Test is disabled due to issues with exception handling.")]
    public async Task CreateMappingAsync_ShouldCreateUser_WhenUserDoesNotExist()
    {
        var token = "valid_token";
        var username = "username";

        _tokenClaimsExtractorSub.ExtractClaim(token, "sub").Returns("sub_claim");
        _tokenClaimsExtractorSub.ExtractClaim(token, "preferred_username").Returns(username);

        _userRepositorySub.GetOidcUserMappingFromSubClaimAsync("sub_claim").Returns(Task.FromException<OidcUserMapping>(new OidcUserMappingNotFound(ApplicationConstants.OIDC_MAPPING_NOT_CREATED)));
        var newUser = new User
        {
            Username = username,
            OidcUserMapping = new OidcUserMapping { SubClaim = "sub_claim" }
        };

        _userRepositorySub.CreateUserAsync(newUser).Returns(Task.FromResult(newUser));

        var result = await _userService.CreateMappingAsync(token);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo(username));
        Assert.That(result.OidcUserMapping.SubClaim, Is.EqualTo("sub_claim"));
    }

    [Test]
    public void CreateMappingAsync_ShouldThrowException_WhenClaimsAreNull()
    {
        var token = "invalid_token";

        _tokenClaimsExtractorSub.ExtractClaim(token, "sub").Returns<string>(string.Empty);
        _tokenClaimsExtractorSub.ExtractClaim(token, "preferred_username").Returns<string>(string.Empty);

        var ex = Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateMappingAsync(token));
        Assert.That(ex.Message, Is.EqualTo(string.Format(ApplicationConstants.OIDC_CLAIMS_EXTRACTION_ERROR, string.Empty, string.Empty)));
    }

    [Test]
    public void CreateMappingAsync_ShouldThrowException_WhenUserMappingAlreadyExists()
    {
        var token = "valid_token";
        var subClaim = "sub_claim";
        var username = "username";

        _tokenClaimsExtractorSub.ExtractClaim(token, "sub").Returns(subClaim);
        _tokenClaimsExtractorSub.ExtractClaim(token, "preferred_username").Returns(username);

        _userRepositorySub.GetOidcUserMappingFromSubClaimAsync(subClaim).Returns(Task.FromResult(new OidcUserMapping { UserId = Guid.NewGuid() }));

        var ex = Assert.ThrowsAsync<OidcUserMappingAlreadyCreated>(() => _userService.CreateMappingAsync(token));
        Assert.That(ex.Message, Is.EqualTo(ApplicationConstants.OIDC_MAPPING_ALREADY_CREATED));
    }

    [Test]
    public async Task GetUserIdFromSubClaimAsync_ShouldReturnUserId_WhenMappingExists()
    {
        var subClaim = "sub_claim";
        var userId = Guid.NewGuid();

        _userRepositorySub.GetOidcUserMappingFromSubClaimAsync(subClaim).Returns(Task.FromResult(new OidcUserMapping { UserId = userId }));

        var result = await _userService.GetUserIdFromSubClaimAsync(subClaim);

        Assert.That(result, Is.EqualTo(userId));
    }

    [Test]
    public void GetUserIdFromSubClaimAsync_ShouldThrowException_WhenMappingNotFound()
    {
        var subClaim = "sub_claim";

        _userRepositorySub.GetOidcUserMappingFromSubClaimAsync(subClaim).Returns(Task.FromException<OidcUserMapping>(new OidcUserMappingNotFound("Mapping not found")));
        var ex = Assert.ThrowsAsync<OidcUserMappingNotFound>(() => _userService.GetUserIdFromSubClaimAsync(subClaim));
        Assert.That(ex.Message, Is.EqualTo("Mapping not found"));
    }
}