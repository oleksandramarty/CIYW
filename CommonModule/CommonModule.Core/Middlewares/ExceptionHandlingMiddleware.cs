using System.Net;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Exceptions.Errors;
using CommonModule.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CommonModule.Core.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AuthException ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex, (HttpStatusCode)ex.statusCode);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex, (HttpStatusCode)ex.statusCode);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError(ex, ErrorMessages.EntityNotFound);
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
        }
        catch (ForbiddenException ex)
        {
            _logger.LogError(ex, ErrorMessages.Forbidden);
            await HandleExceptionAsync(context, ex, HttpStatusCode.Forbidden);
        }
        catch (VersionException ex)
        {
            _logger.LogError(ex, ErrorMessages.VersionNotSpecified);
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
        }
        catch (BaseException ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex, (HttpStatusCode)ex.statusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var errorModel = new ErrorMessageModel(exception.Message, context.Response.StatusCode);
        var result = errorModel.ToJson();

        return context.Response.WriteAsync(result);
    }
}