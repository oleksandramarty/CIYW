using System.Net;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Exceptions.Errors;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums.AuditTrail;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CommonModule.Core.Middlewares;

/// <summary>
/// Middleware for handling exceptions in the application.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger to log exceptions.</param>
    /// <param name="auditTrailRepository">The audit trail repository.</param>
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHttpContextAccessor httpContextAccessor
    )
    {
        this.next = next;
        this.logger = logger;
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
            this.logger.LogError(ex, ex.Message);
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.AuthException, ex.Message);
            await HandleExceptionAsync(context, ex, (HttpStatusCode)ex.statusCode);
        }
        catch (BusinessException ex)
        {
            this.logger.LogError(ex, ex.Message);
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.BusinessException, ex.Message);
            await HandleExceptionAsync(context, ex, (HttpStatusCode)ex.statusCode);
        }
        catch (EntityNotFoundException ex)
        {
            this.logger.LogError(ex, ErrorMessages.EntityNotFound);
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.EntityNotFoundException, ex.Message);
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
        }
        catch (ForbiddenException ex)
        {
            this.logger.LogError(ex, ErrorMessages.Forbidden);
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.ForbiddenException, ex.Message);
            await HandleExceptionAsync(context, ex, HttpStatusCode.Forbidden);
        }
        catch (VersionException ex)
        {
            this.logger.LogError(ex, ErrorMessages.VersionNotSpecified);
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.VersionException, ex.Message);
            await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
        }
        catch (BaseException ex)
        {
            this.logger.LogError(ex, ex.Message);
            await this.CreateAuditTrailAsync(context, ExceptionTypeEnum.BaseException, ex.Message);
            await HandleExceptionAsync(context, ex, (HttpStatusCode)ex.statusCode);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);
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
        // await this.auditTrailDataContext.AuditTrail.AddAsync(new AuditTrailEntity
        // {
        //     Id = Guid.NewGuid(),
        //     CreatedAt = DateTime.UtcNow,
        //     Type = AuditTrailTypeEnum.Error,
        //     ExceptionType = exceptionType,
        //     Message = message,
        //     Payload = context.Request.Body != null
        //         ? await new StreamReader(context.Request.Body).ReadToEndAsync()
        //         : null,
        //     UserId = this.httpContextAccessor.HttpContext?.User.FindFirst(AuthClaims.UserId)?.Value != null
        //         ? Guid.Parse(this.httpContextAccessor.HttpContext.User.FindFirst(AuthClaims.UserId).Value)
        //         : (Guid?)null
        // }, CancellationToken.None);
        // await this.auditTrailDataContext.SaveChangesAsync();
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