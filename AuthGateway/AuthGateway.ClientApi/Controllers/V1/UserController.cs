using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.Core;
using CommonModule.Shared.Responses.AuthGateway.Users;
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
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        UserResponse response = await this.mediator.Send(new GetCurrentUserRequest());
        return Ok(response);
    }
}