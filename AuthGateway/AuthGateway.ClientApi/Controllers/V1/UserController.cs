using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.Core;
using CommonModule.Facade;
using CommonModule.Shared.Responses.SiteSettings;
using CommonModule.Shared.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthGateway.ClientApi.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/users")]
public class UserController : BaseController
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpGet("current")]
    [ProducesResponseType(typeof(UserResponse), 200)]
    public async Task<IActionResult> GetUserId()
    {
        UserResponse response = await this.mediator.Send(new GetCurrentUserRequest());
        return Ok(response);
    }
    
    [HttpGet("settings")]
    [ProducesResponseType(typeof(SiteSettingsResponse), 200)]
    public IActionResult GetVersion()
    {
        SiteSettingsResponse response = new SiteSettingsResponse
        {
            Locale = "en",
            ApiVersion = VersionGenerator.GetVersion(),
            ClientVersion = string.Empty
        };
        return Ok(response);
    }
}