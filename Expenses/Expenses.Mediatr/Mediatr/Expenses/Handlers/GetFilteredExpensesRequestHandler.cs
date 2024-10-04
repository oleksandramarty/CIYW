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

public class GetFilteredExpensesRequestHandler: MediatrAuthBase, IRequestHandler<GetFilteredExpensesRequest, ListWithIncludeResponse<ExpenseResponse>>
{
    private readonly IEntityValidator<ExpensesDataContext> entityValidator;
    private readonly IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository;
    private readonly IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository;

    public GetFilteredExpensesRequestHandler(
        IAuthRepository authRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository
        ): base(authRepository)
    {
        this.entityValidator = entityValidator;
        this.expenseRepository = expenseRepository;
        this.userProjectRepository = userProjectRepository;
    }

    public async Task<ListWithIncludeResponse<ExpenseResponse>> Handle(GetFilteredExpensesRequest request, CancellationToken cancellationToken)
    {
        Guid userId = await this.GetCurrentUserIdAsync();
        
        UserProject userProject =
            await this.userProjectRepository.GetAsync(
                up => up.Id == request.UserProjectId, 
                cancellationToken,
                up => up.Include(a => a.AllowedUsers).Include(b => b.Balances));
        this.entityValidator.ValidateExist<UserProject, Guid>(userProject, request.UserProjectId);
        
        if (userProject.CreatedUserId != userId && userProject.AllowedUsers.All(au => au.UserId != userId))
        {
            throw new ForbiddenException();
        }
        
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