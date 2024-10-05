using CommonModule.Core;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.ClientApi.Controllers.V1;

[ApiController]
[Authorize]
[Route("api/v1/planned-expenses")]
public class PlannedExpenseController: BaseController
{
    private readonly IMediator mediator;

    public PlannedExpenseController(IMediator mediator) : base(mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> CreateOrUpdatePlannedExpenseAsync([FromBody]CreateOrUpdatePlannedExpenseCommand request, CancellationToken cancellationToken)
    {
        await mediator.Send(request, cancellationToken);
        return Ok();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> RemovePlannedExpenseAsync(Guid id, CancellationToken cancellationToken)
    {
        bool result = await mediator.Send(new RemovePlannedExpenseCommand { Id = id }, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("filter")]
    [ProducesResponseType(typeof(ListWithIncludeResponse<PlannedExpenseResponse>), 200)]
    public async Task<IActionResult> GetFilteredPlannedExpensesAsync([FromBody]GetFilteredPlannedExpensesRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}