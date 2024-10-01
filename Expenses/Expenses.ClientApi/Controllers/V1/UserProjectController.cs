using CommonModule.Core;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.ClientApi.Controllers.V1;

[ApiController]
[Authorize]
[Route("api/v1/user-projects")]
public class UserProjectController: BaseController
{
    private readonly IMediator mediator;

    public UserProjectController(IMediator mediator) : base(mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpGet("allowed")]
    [ProducesResponseType(typeof(List<UserAllowedProjectResponse>), 200)]
    public async Task<IActionResult> GetAllowedProjectsAsync(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetUserAllowedProjectsRequest(), cancellationToken);
        return Ok(response);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<UserProjectResponse>), 200)]
    public async Task<IActionResult> GetProjectsAsync(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetUserProjectsRequest(), cancellationToken);
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> AddProjectAsync([FromBody]CreateUserProjectCommand request, CancellationToken cancellationToken)
    {
        await mediator.Send(request, cancellationToken);
        return Ok();
    }
}