using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.Core;
using CommonModule.Shared.Responses.Auth;
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
    [ProducesResponseType(typeof(JwtTokenResponse), 200)]
    public async Task<IActionResult> SignInAsync([FromBody]AuthSignInRequest request, CancellationToken cancellationToken)
    {
        JwtTokenResponse response = await this.mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("signOut")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> SignOutAsync(CancellationToken cancellationToken)
    {
        await this.mediator.Send(new AuthSignOutRequest(), cancellationToken);
        return Ok();
    }
    
    [HttpPost("signUp")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> SignUpAsync([FromBody]AuthSignUpCommand command, CancellationToken cancellationToken)
    {
        await this.mediator.Send(command, cancellationToken);
        return Ok();
    }
}