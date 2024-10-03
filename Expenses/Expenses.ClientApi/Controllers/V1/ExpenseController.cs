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
[Route("api/v1/expenses")]
public class ExpenseController: BaseController
{
    private readonly IMediator mediator;

    public ExpenseController(IMediator mediator) : base(mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> CreateOrUpdateExpenseAsync([FromBody]CreateOrUpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        await mediator.Send(request, cancellationToken);
        return Ok();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> RemoveExpenseAsync(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveExpenseCommand { Id = id }, cancellationToken);
        return Ok();
    }
    
    [HttpPost("filter")]
    [ProducesResponseType(typeof(ListWithIncludeResponse<ExpenseResponse>), 200)]
    public async Task<IActionResult> GetFilteredExpensesAsync([FromBody]GetFilteredExpensesRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}