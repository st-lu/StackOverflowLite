using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow_Lite.Services;
using Stackoverflow_Lite.Configurations;
using Stackoverflow_Lite.models;
using Stackoverflow_Lite.Services;
using Stackoverflow_Lite.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace Stackoverflow_Lite.Controllers;

[ApiController]
[Route("/answer")]
public class AnswerController : ControllerBase
{
    private readonly IAnswerService _answerService;

    public AnswerController(IAnswerService answerService)
    {
        _answerService = answerService;
    }

    [Authorize]
    [HttpPost]
    [SwaggerOperation(Summary = "Create answer", Description = "Creates an answer given the question Id and content")]
    [SwaggerResponse(201, "Answer created successfully")]
    [SwaggerResponse(401, "Unauthorized user")]
    [SwaggerResponse(400, "Mapping not created/ Validation errors")]
    public async Task<IActionResult> CreateAnswer([Required][FromQuery] Guid questionId,[FromBody] AnswerRequest answerRequest)
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        var answer = await _answerService.CreateAnswerAsync(token, questionId, answerRequest);
        var uri = Url.Action("GetAnswer", new { answerId = answer.Id });
        return Created(uri, answer);

    }
    [Authorize]
    [HttpPut]
    [Route("{answerId}")]
    [SwaggerOperation(Summary = "Edit an answer", Description = "Edit an answer given the answer Id and new content")]
    [SwaggerResponse(200, "Answer modified successfully")]
    [SwaggerResponse(401, "Unauthorized user")]
    [SwaggerResponse(400, "Mapping not created/ Validation errors")]
    [SwaggerResponse(403, "User is not the owner of the resource")]

    public async Task<IActionResult> EditAnswer(Guid answerId, [FromBody] AnswerRequest answerRequest)
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        return Ok(await _answerService.EditAnswerAsync(token, answerId, answerRequest));
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    [Route("admin/{answerId}")]
    [SwaggerOperation(Summary = "Deletes an answer as admin", Description = "Deletes an answer given the answer Id ")]
    [SwaggerResponse(200, "Answer deleted successfully")]
    [SwaggerResponse(401, "Unauthorized user")]
    [SwaggerResponse(400, "Mapping not created/ Validation errors")]
    [SwaggerResponse(403, "User is not an admin")]
    public async Task<IActionResult> DeleteAnswerAdmin(Guid answerId)
    {
        await _answerService.DeleteAnswerAdminAsync(answerId);
        return Ok(ApplicationConstants.ANSWER_SUCCESSFULLY_DELETED);
    }
    [Authorize]
    [HttpDelete]
    [Route("{answerId}")]
    [SwaggerOperation(Summary = "Delete an answer", Description = "Delete an answer given the answer")]
    [SwaggerResponse(200, "Answer deleted successfully")]
    [SwaggerResponse(401, "Unauthorized user")]
    [SwaggerResponse(400, "Mapping not created/ Validation errors")]
    [SwaggerResponse(403, "User is not the owner of the resource")]
    public async Task<IActionResult> DeleteAnswer(Guid answerId)
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        await _answerService.DeleteAnswerAsync(token,answerId);
        return Ok(ApplicationConstants.QUESTION_SUCCESSFULLY_DELETED);
    }
}