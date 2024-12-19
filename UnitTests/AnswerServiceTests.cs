using NSubstitute;
using Stackoverflow_Light.Repositories;
using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.Exceptions;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Services.Interfaces;
using Stackoverflow_Lite.Services;
using Stackoverflow_Lite.Utils.Interfaces;
using Stackoverflow_Lite.Utils;

namespace UnitTests;

[TestFixture]
public class AnswerServiceTests
{
    private IAnswerRepository _answerRepository;
    private ITokenClaimsExtractor _tokenClaimsExtractor;
    private IUserService _userService;
    private AnswerService _answerService;

    [SetUp]
    public void Setup()
    {
        _answerRepository = Substitute.For<IAnswerRepository>();
        _tokenClaimsExtractor = Substitute.For<ITokenClaimsExtractor>();
        _userService = Substitute.For<IUserService>();
        _answerService = new AnswerService(_answerRepository, _tokenClaimsExtractor, _userService);
    }

    [Test]
    public async Task CreateAnswerAsync_ShouldReturnAnswer_WhenValidRequest()
    {
        var token = "token";
        var questionId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var answerRequest = new AnswerRequest() { Content = "Test Content" };

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(userId));

        var answer = new Answer
        {
            Content = answerRequest.Content,
            UserId = userId,
            QuestionId = questionId
        };

        _answerRepository.CreateAnswerAsync(Arg.Any<Answer>()).Returns(Task.FromResult(answer));

        // calling the function to be tested
        var result = await _answerService.CreateAnswerAsync(token, questionId, answerRequest);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Content, Is.EqualTo(answerRequest.Content));
        Assert.That(result.UserId, Is.EqualTo(userId));
        Assert.That(result.QuestionId, Is.EqualTo(questionId));
    }
    [Test]
    public void CreateAnswerAsync_ShouldThrowException_WhenUserIdCannotBeRetrieved()
    {
        var token = "invalid-token";
        var questionId = Guid.NewGuid();
        var answerRequest = new AnswerRequest { Content = "Test Content" };

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromException<Guid>(new Exception("User not found")));

        Assert.ThrowsAsync<Exception>(async () => await _answerService.CreateAnswerAsync(token, questionId, answerRequest));
    }

    [Test]
    public async Task DeleteAnswerAsync_ShouldDeleteAnswer_WhenUserIsAuthor()
    {
        var token = "valid-token";
        var answerId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(userId));
        _answerRepository.GetAuthorIdFromAnswerIdAsync(answerId).Returns(Task.FromResult(userId));

        await _answerService.DeleteAnswerAsync(token, answerId);

        await _answerRepository.Received().DeleteAnswerAsync(answerId);
    }

    [Test]
    public async Task DeleteAnswerAsync_ShouldThrowOperationNotAllowed_WhenUserIsNotAuthor()
    {
        var token = "valid-token";
        var answerId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(userId));
        // returns a different UserID
        _answerRepository.GetAuthorIdFromAnswerIdAsync(answerId).Returns(Task.FromResult(Guid.NewGuid()));

        var ex = Assert.ThrowsAsync<OperationNotAllowed>(async () => await _answerService.DeleteAnswerAsync(token, answerId));
        Assert.That(ex.Message, Is.EqualTo(ApplicationConstants.OPERATION_NOT_ALLOWED_MESSAGE));
    }

    [Test]
    public async Task EditAnswerAsync_ShouldReturnUpdatedAnswer_WhenUserIsAuthor()
    {
        var token = "valid-token";
        var answerId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var answerRequest = new AnswerRequest { Content = "Updated Content" };
        var updatedAnswer = new Answer
        {
            Content = answerRequest.Content,
            UserId = userId,
            QuestionId = Guid.NewGuid()
        };

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(userId));
        _answerRepository.GetAuthorIdFromAnswerIdAsync(answerId).Returns(Task.FromResult(userId));
        _answerRepository.EditAnswerAsync(answerId, answerRequest).Returns(Task.FromResult(updatedAnswer));

        var result = await _answerService.EditAnswerAsync(token, answerId, answerRequest);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Content, Is.EqualTo(answerRequest.Content));
    }

    [Test]
    public void EditAnswerAsync_ShouldThrowOperationNotAllowed_WhenUserIsNotAuthor()
    {
        var token = "valid-token";
        var answerId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var answerRequest = new AnswerRequest { Content = "Updated Content" };

        _tokenClaimsExtractor.ExtractClaim(token, "sub").Returns("subClaim");
        _userService.GetUserIdFromSubClaimAsync("subClaim").Returns(Task.FromResult(userId));
        _answerRepository.GetAuthorIdFromAnswerIdAsync(answerId).Returns(Task.FromResult(Guid.NewGuid())); // Different userId

        var ex = Assert.ThrowsAsync<OperationNotAllowed>(async () => await _answerService.EditAnswerAsync(token, answerId, answerRequest));
        Assert.That(ex.Message, Is.EqualTo(ApplicationConstants.OPERATION_NOT_ALLOWED_MESSAGE));
    }

    [Test]
    public async Task DeleteAnswerAdminAsync_ShouldDeleteAnswer()
    {
        var answerId = Guid.NewGuid();

        await _answerService.DeleteAnswerAdminAsync(answerId);

        await _answerRepository.Received().DeleteAnswerAsync(answerId);
    }
}
