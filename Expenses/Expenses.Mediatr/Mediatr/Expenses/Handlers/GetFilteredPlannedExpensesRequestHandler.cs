using CommonModule.Core.Extensions;
using CommonModule.Core.Strategies.GetFilteredResult;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class GetFilteredPlannedExpensesRequestHandler: MediatrExpensesBase, IRequestHandler<GetFilteredPlannedExpensesRequest, ListWithIncludeResponse<PlannedExpenseResponse>>
{
    private readonly IGetFilteredResultStrategy<GetFilteredPlannedExpensesRequest, PlannedExpenseResponse> strategy;

    public GetFilteredPlannedExpensesRequestHandler(
        IAuthRepository authRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository,
        IGetFilteredResultStrategy<GetFilteredPlannedExpensesRequest, PlannedExpenseResponse> strategy
    ): base(authRepository, entityValidator, userProjectRepository)
    {
        this.strategy = strategy;
    }

    public async Task<ListWithIncludeResponse<PlannedExpenseResponse>> Handle(GetFilteredPlannedExpensesRequest request, CancellationToken cancellationToken)
    {
        await this.CheckUserProjectByIdAsync(request.UserProjectId, cancellationToken);
        
        request.CheckBaseFilter();
        request.CategoryIds.CheckIds();
        
        return await this.strategy.GetFilteredResultAsync(request, cancellationToken);
    }
}