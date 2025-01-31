using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow_Lite.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Stackoverflow_Lite.controllers;

[ApiController]
[Route("/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [Authorize]
    [HttpPost("/create-mapping")]
    [SwaggerOperation(Summary = "Creates an oidc-user-mapping", Description = "Creates a mapping between the OIDC user and local representation based on the token received")]
    [SwaggerResponse(200, "Mapping created successfully")]
    [SwaggerResponse(400, "Claim Extraction Error")]
    public async Task<IActionResult> CreateMapping()
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        var user = await _userService.CreateMappingAsync(token);
        return Ok(user);
    }
    
    [Authorize]
    [HttpGet("/me/questions")]
    [SwaggerOperation(Summary = "Get all questions of an user", Description = "Get question details with one unparametrized request")]
    [SwaggerResponse(200, "Question fetched successfully")]
    [SwaggerResponse(401, "Unauthorized user")]
    public async Task<IActionResult> GetUserAllQuestions()
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        var questions = await _userService.GetAllUserQuestions(token);
        return Ok(questions);
    }
    
    [Authorize]
    [HttpGet("/current")]
    [SwaggerOperation(Summary = "Get current user", Description = "Get current user or 404")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        var user = await _userService.GetUserAsync(token);
        return Ok(user);
    }
    
    [Authorize]
    [HttpGet("/most-active-users")]
    [SwaggerOperation(Summary = "Get most active users, sorted by number of posts", Description = "Get active users")]
    public async Task<IActionResult> GetMostActiveUsers()
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        var users = await _userService.GetMostActiveUsers(token);
        return Ok(users);
    }
    
    [Authorize]
    [HttpGet("/is-admin")]
    [SwaggerOperation(Summary = "Check if logged in user is Admin or not", Description = "Get User Role")]
    public async Task<IActionResult> IsAdmin()
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        var isAdmin = await _userService.IsAdmin(token);
        return Ok(isAdmin);
    }
    
}