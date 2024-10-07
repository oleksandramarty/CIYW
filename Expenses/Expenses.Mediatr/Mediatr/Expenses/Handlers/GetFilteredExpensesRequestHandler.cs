using CommonModule.Core.Extensions;
using CommonModule.Core.Strategies.GetFilteredResult;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class GetFilteredExpensesRequestHandler : MediatrExpensesBase,
    IRequestHandler<GetFilteredExpensesRequest, ListWithIncludeResponse<ExpenseResponse>>
{
    private readonly IGetFilteredResultStrategy<GetFilteredExpensesRequest, ExpenseResponse> strategy;

    public GetFilteredExpensesRequestHandler(
        IAuthRepository authRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository,
        IGetFilteredResultStrategy<GetFilteredExpensesRequest, ExpenseResponse> strategy
    ) : base(authRepository, entityValidator, userProjectRepository)
    {
        this.strategy = strategy;
    }

    public async Task<ListWithIncludeResponse<ExpenseResponse>> Handle(GetFilteredExpensesRequest request,
        CancellationToken cancellationToken)
    {
        await this.CheckUserProjectByIdAsync(request.UserProjectId, cancellationToken);

        request.CheckBaseFilter();
        request.CategoryIds.CheckIds();

        return await this.strategy.GetFilteredResultAsync(request, cancellationToken);
    }
}