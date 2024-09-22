using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthGateway.ClientApi.Controllers.V1;

[ApiController]
[Route("api/v1/auth")]
public class AuthController: BaseController
{
    private readonly IMediator mediator;
    
    public AuthController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpPost("signIn")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> SignInAsync(AuthSignInRequest request, CancellationToken cancellationToken)
    {
        await this.mediator.Send(request, cancellationToken);
        return Ok();
    }
    
    [HttpPost("signOut")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> SignOutAsync(AuthSignOutRequest request, CancellationToken cancellationToken)
    {
        await this.mediator.Send(request, cancellationToken);
        return Ok();
    }
    
    [HttpPost("signUp")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> SignUpAsync(AuthSignUpCommand command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);
        return Ok();
    }
}