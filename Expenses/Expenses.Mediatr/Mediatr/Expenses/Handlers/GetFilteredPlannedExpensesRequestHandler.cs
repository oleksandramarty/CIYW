using CommonModule.Core.Extensions;
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
    private readonly IReadGenericRepository<Guid, PlannedExpense, ExpensesDataContext> plannedExpenseRepository;

    public GetFilteredPlannedExpensesRequestHandler(
        IAuthRepository authRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, PlannedExpense, ExpensesDataContext> plannedExpenseRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
    ): base(authRepository, entityValidator, userProjectRepository)
    {
        this.plannedExpenseRepository = plannedExpenseRepository;
    }

    public async Task<ListWithIncludeResponse<PlannedExpenseResponse>> Handle(GetFilteredPlannedExpensesRequest request, CancellationToken cancellationToken)
    {
        await this.CheckUserProjectByIdAsync(request.UserProjectId, cancellationToken);
        
        request.DateRange.CheckOrApplyDefaultExpenseFilter();
        
        return await this.plannedExpenseRepository.GetListWithIncludeAsync<PlannedExpenseResponse>(
            e => e.UserProjectId == request.UserProjectId && 
                 (string.IsNullOrEmpty(request.Query) || 
                  !string.IsNullOrEmpty(request.Query) && EF.Functions.Like(e.Title, $"%{request.Query}%") ||
                  !string.IsNullOrEmpty(request.Query) && EF.Functions.Like(e.Description, $"%{request.Query}%")
                 ),
            request,
            cancellationToken);
    }
}