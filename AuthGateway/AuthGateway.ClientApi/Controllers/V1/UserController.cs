using AuthGateway.Mediatr.Mediatr.Auth.Commands;
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

    public UserController(IMediator mediator) : base(mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpPost("settings")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> CreateOrUpdateUserSettingAsync([FromBody] CreateOrUpdateUserSettingCommand command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);
        return Ok();
    }
}