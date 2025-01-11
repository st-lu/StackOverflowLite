using Stackoverflow_Lite.Exceptions;

namespace Stackoverflow_Lite.Utils;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (EntityNotFound ex )
        {
            _logger.LogError(ex, "Entity not found.");

            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
        }
        catch (OidcUserMappingAlreadyCreated ex )
        {
            _logger.LogError(ex, "Mapping already created.");

            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
        }
        catch (OidcUserMappingNotFound ex )
        {
            _logger.LogError(ex, "Entity not found.");

            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
        }
        catch (OperationNotAllowed ex )
        {
            _logger.LogError(ex, "Operation not allowed.");

            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
        }
        catch (Exception ex )
        {
            _logger.LogError(ex, "An unknown error occured.");

            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var errorResponse = new
        {
            exception.Message
        };

        var errorJson = JsonSerializer.Serialize(errorResponse);

        return context.Response.WriteAsync(errorJson);
    }
}
