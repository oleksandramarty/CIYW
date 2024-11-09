using System.Net;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Exceptions.Errors;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums.AuditTrail;
using Microsoft.AspNetCore.Http;

namespace AuditTrail.Business;

/// <summary>
/// Middleware for handling exceptions in the application.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly IAuditTrailRepository auditTrailRepository;
    private readonly IHttpContextAccessor httpContextAccessor;
    
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        IAuditTrailRepository auditTrailRepository,
        IHttpContextAccessor httpContextAccessor
    )
    {
        this.next = next;
        this.auditTrailRepository = auditTrailRepository;
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Invokes the middleware to handle exceptions.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (AuthException ex)
        {
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.AuthException, ex.Message);
            await HandleExceptionAsync(context, ex, (HttpStatusCode)ex.statusCode);
        }
        catch (BusinessException ex)
        {
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.BusinessException, ex.Message);
            await HandleExceptionAsync(context, ex, (HttpStatusCode)ex.statusCode);
        }
        catch (EntityNotFoundException ex)
        {
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.EntityNotFoundException, ex.Message);
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
        }
        catch (ForbiddenException ex)
        {
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.ForbiddenException, ex.Message);
            await HandleExceptionAsync(context, ex, HttpStatusCode.Forbidden);
        }
        catch (VersionException ex)
        {
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.VersionException, ex.Message);
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
        }
        catch (BaseException ex)
        {
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.BaseException, ex.Message);
            await HandleExceptionAsync(context, ex, (HttpStatusCode)ex.statusCode);
        }
        catch (Exception ex)
        {
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.Exception, ex.Message);
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    /// Creates an audit trail for the exception.
    /// </summary>
    /// <param name="context">Request context</param>
    /// <param name="exceptionType">Type of the exception</param>
    /// <param name="message">Message of the exception</param>
    private async Task CreateAuditTrailAsync(
        HttpContext context, ExceptionTypeEnum exceptionType, string message
    )
    {
        await this.auditTrailRepository.AddExceptionLogAsync(
            this.httpContextAccessor.HttpContext?.User.FindFirst(AuthClaims.UserId)?.Value != null
                ? Guid.Parse(this.httpContextAccessor.HttpContext.User.FindFirst(AuthClaims.UserId).Value)
                : (Guid?)null,
            exceptionType,
            message,
            context.Request.Body != null
                ? await new StreamReader(context.Request.Body).ReadToEndAsync()
                : null,
            CancellationToken.None
        );
    }

    /// <summary>
    /// Handles the exception and writes the response.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="exception">The exception to handle.</param>
    /// <param name="statusCode">The HTTP status code to return.</param>
    /// <returns>A task that represents the completion of response writing.</returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var errorModel = new ErrorMessageModel(exception.Message, context.Response.StatusCode);
        var result = errorModel.ToJson();

        return context.Response.WriteAsync(result);
    }
}