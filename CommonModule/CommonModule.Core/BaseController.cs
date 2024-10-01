using CommonModule.Core.Exceptions.Errors;
using CommonModule.Core.Mediatr.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommonModule.Core;

[Route("api/{version}/[controller]")]
[ApiController]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status409Conflict)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status417ExpectationFailed)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status500InternalServerError)]
public class BaseController : Controller
{
    private readonly IMediator mediator;
    
    public BaseController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    protected async Task<Guid?> GetCurrentUserIdAsync(CancellationToken cancellationToken)
    {
        return await this.mediator.Send(new GetUserIdRequest(), cancellationToken);
    }
}