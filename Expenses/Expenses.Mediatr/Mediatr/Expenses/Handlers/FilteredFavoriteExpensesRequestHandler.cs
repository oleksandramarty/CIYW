using CommonModule.Core.Extensions;
using CommonModule.Core.Strategies.FilteredResult;
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

public class FilteredFavoriteExpensesRequestHandler: MediatrExpensesBase, IRequestHandler<FilteredFavoriteExpensesRequest, FilteredListResponse<FavoriteExpenseResponse>>
{
    private readonly IFilteredResultStrategy<FilteredFavoriteExpensesRequest, FavoriteExpenseResponse> _strategy;

    public FilteredFavoriteExpensesRequestHandler(
        ICurrentUserRepository currentUserRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProjectEntity, ExpensesDataContext> readGenericUserProjectRepository,
        IFilteredResultStrategy<FilteredFavoriteExpensesRequest, FavoriteExpenseResponse> strategy
    ): base(currentUserRepository, entityValidator, readGenericUserProjectRepository)
    {
        _strategy = strategy;
    }

    public async Task<FilteredListResponse<FavoriteExpenseResponse>> Handle(FilteredFavoriteExpensesRequest request, CancellationToken cancellationToken)
    {
        await CheckUserProjectByIdAsync(request.UserProjectId, cancellationToken);
        
        request.CheckBaseFilter();
        request.CategoryIds.CheckIds();
        
        return await _strategy.FilteredResultAsync(request, cancellationToken);
    }
}