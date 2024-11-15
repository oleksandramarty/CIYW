using Microsoft.AspNetCore.Http;

namespace CIYW.IntegrationTests.Core;

/// <summary>
/// Utility class for testing MediatR commands
/// </summary>
public class HttpContextAccessorForTesting : IHttpContextAccessor
{
    public HttpContext HttpContext { get; set; }
}