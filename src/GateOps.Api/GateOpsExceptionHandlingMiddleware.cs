using GateOps.Application.GateOperations;
using GateOps.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace GateOps.Api;

/// <summary>Translates domain/application exceptions into the right HTTP status
/// code + a standard ProblemDetails body, so controllers stay free of try/catch
/// boilerplate for expected business-rule failures.</summary>
public sealed class GateOpsExceptionHandlingMiddleware(RequestDelegate next, ILogger<GateOpsExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AppointmentNotFoundException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status404NotFound, "Appointment not found", ex.Message);
        }
        catch (DomainException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Invalid gate operation", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception processing {Method} {Path}", context.Request.Method, context.Request.Path);
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Unexpected error", "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        var problem = new ProblemDetails { Status = statusCode, Title = title, Detail = detail };
        await context.Response.WriteAsJsonAsync(problem);
    }
}
