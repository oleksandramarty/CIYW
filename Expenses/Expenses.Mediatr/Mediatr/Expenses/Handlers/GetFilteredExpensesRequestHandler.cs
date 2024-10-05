using CommonModule.Core.Exceptions;
using CommonModule.Core.Extensions;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Mediatr.Expenses.Handlers;

public class GetFilteredExpensesRequestHandler: MediatrExpensesBase, IRequestHandler<GetFilteredExpensesRequest, ListWithIncludeResponse<ExpenseResponse>>
{
    private readonly IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository;

    public GetFilteredExpensesRequestHandler(
        IAuthRepository authRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
        ): base(authRepository, entityValidator, userProjectRepository)
    {
        this.expenseRepository = expenseRepository;
    }

    public async Task<ListWithIncludeResponse<ExpenseResponse>> Handle(GetFilteredExpensesRequest request, CancellationToken cancellationToken)
    {
        await this.CheckUserProjectByIdAsync(request.UserProjectId, cancellationToken);
        
        request.DateRange.CheckOrApplyDefaultExpenseFilter();
        
        return await this.expenseRepository.GetListWithIncludeAsync<ExpenseResponse>(
            e => e.UserProjectId == request.UserProjectId && 
                 (string.IsNullOrEmpty(request.Query) || 
                  !string.IsNullOrEmpty(request.Query) && EF.Functions.Like(e.Title, $"%{request.Query}%") ||
                    !string.IsNullOrEmpty(request.Query) && EF.Functions.Like(e.Description, $"%{request.Query}%")
                  ) &&
            (request.CategoryIds.Count == 0 || request.CategoryIds.Count != 0 && request.CategoryIds.Contains(e.CategoryId)),
            request,
            cancellationToken);
    }
}