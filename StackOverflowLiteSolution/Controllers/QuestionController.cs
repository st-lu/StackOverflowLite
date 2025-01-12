using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow_Lite.Services.Interfaces;
using Stackoverflow_Lite.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Stackoverflow_Lite.Strategies;
using Stackoverflow_Lite.Strategies.Interfaces;
using Stackoverflow_Lite.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace Stackoverflow_Lite.Controllers;


[ApiController]
[Route("/question")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [Authorize]
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new question", Description = "Creates a new question with the given content")]
    [SwaggerResponse(202, "Question was accepted")]
    [SwaggerResponse(404, "Question was not found in the DB")]
    [SwaggerResponse(401, "Unauthorized user")]
    [SwaggerResponse(400, "Mapping not created/ Validation errors")]
    public async Task<IActionResult> CreateQuestion([FromBody] QuestionRequest questionRequest)
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        var questionId = await _questionService.CreateQuestionAsync(token, questionRequest);
        return Accepted(questionId);

    }

    [HttpGet("{questionId}")]
    [SwaggerOperation(Summary = "Get question details", Description = "Get question details with the given Id")]
    [SwaggerResponse(200, "Question fetched successfully")]
    [SwaggerResponse(404, "Question was not found in the DB")]
    [SwaggerResponse(401, "Unauthorized user")]
    [SwaggerResponse(400, "Mapping not created")]
    public async Task<IActionResult> GetQuestion(Guid questionId)
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        return Ok(await _questionService.GetQuestionAsync(token,questionId));
    }

    [Authorize]
    [HttpDelete]
    [Route("{questionId}")]
    [SwaggerOperation(Summary = "Delete question", Description = "Deletes the question with the given Id")]
    [SwaggerResponse(200, "Question deleted successfully")]
    [SwaggerResponse(404, "Question was not found in the DB")]
    [SwaggerResponse(401, "Unauthorized user")]
    [SwaggerResponse(403, "User is not the owner of the resource")]
    [SwaggerResponse(400, "Mapping not created")]

    public async Task<IActionResult> DeleteQuestion(Guid questionId)
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        await _questionService.DeleteQuestionAsync(token,questionId);
        return Ok(ApplicationConstants.QUESTION_SUCCESSFULLY_DELETED);
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete]
    [Route("admin/{questionId}")]
    [SwaggerOperation(Summary = "Delete question as admin", Description = "Deletes the question with the given Id")]
    [SwaggerResponse(200, "Question deleted successfully")]
    [SwaggerResponse(404, "Question was not found in the DB")]
    [SwaggerResponse(401, "Unauthorized user")]
    [SwaggerResponse(403, "User is not an admin")]
    [SwaggerResponse(400, "Mapping not created")]

    public async Task<IActionResult> DeleteQuestionAdmin(Guid questionId)
    {
        await _questionService.DeleteQuestionAdminAsync(questionId);
        return Ok(ApplicationConstants.QUESTION_SUCCESSFULLY_DELETED);
    }

    [Authorize]
    [HttpPut]
    [Route("{questionId}")]
    [SwaggerOperation(Summary = "Modify question", Description = "Modifies the question with the given Id")]
    [SwaggerResponse(200, "Question modified successfully")]
    [SwaggerResponse(404, "Question was not found in the DB")]
    [SwaggerResponse(401, "Unauthorized user")]
    [SwaggerResponse(403, "User is not the owner of the resource")]
    [SwaggerResponse(400, "Mapping not created/ Validation errors")]

    public async Task<IActionResult> EditQuestion(Guid questionId, [FromBody] QuestionRequest questionRequest)
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        return Ok(await _questionService.EditQuestionAsync(token, questionId, questionRequest));
    }
    [HttpGet]
    [SwaggerOperation(Summary = "Get batch of questions", Description = "Get the requested batch of questions sorted by popularity")]
    [SwaggerResponse(200, "Questions fetched successfully")]
    public async Task<IActionResult> GetQuestions(
        [Required][FromQuery] [SwaggerParameter(Description = "Starting index for the batch of questions")]int offset, 
        [Required][FromQuery] [SwaggerParameter(Description = "Number of questions to fetch")]int size,
        [FromQuery][SwaggerParameter(Description = "Key words to look for")] string? searchText,
        [FromQuery][SwaggerParameter(Description = "Sort Ascending(0)/Descending(1) based on unique number of viewers")] string? viewsCountOrder,
        [FromQuery][SwaggerParameter(Description = "Sort Ascending(0)/Descending(1) based on score")]string? scoreOrder)
    {
        var strategies = new List<IQuestionFilterStrategy>();
        
        if (!string.IsNullOrEmpty(searchText))
        {
            strategies.Add(new TextFilterStrategy(searchText));
        }

        if (!string.IsNullOrEmpty(viewsCountOrder))
        {
            bool ascending = viewsCountOrder.ToLower() == "0";
            strategies.Add(new ViewsCountStrategy(ascending));
        }
        if (!string.IsNullOrEmpty(scoreOrder))
        {
            strategies.Add(new ScoreStrategy(scoreOrder.ToLower() == "0"));
        }
        
        IEnumerable<QuestionDto> questions;
        
        if (!strategies.IsNullOrEmpty())
        {
            questions = await _questionService.GetFilteredQuestionsAsync(strategies);
        }
        else
        {
            questions = await _questionService.GetQuestionsAsync(offset, size);
        }
        
        return Ok(questions);
    }

    [HttpPost("vote")]
    [SwaggerOperation(Summary = "Upvote or Downvote question", Description = "Parameter Vote takes values 1 (for upvote) or -1 (for downvote)")]

    public async Task<IActionResult> VoteQuestion([FromBody] QuestionVoteRequest questionVoteRequest)
    {
        var token = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
        var question = await _questionService.VoteQuestionAsync(questionVoteRequest);
        return Ok(question);
    }
    
    
}