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

public class GetFilteredFavoriteExpensesRequestHandler: MediatrExpensesBase, IRequestHandler<GetFilteredFavoriteExpensesRequest, FilteredListResponse<FavoriteExpenseResponse>>
{
    private readonly IGetFilteredResultStrategy<GetFilteredFavoriteExpensesRequest, FavoriteExpenseResponse> strategy;

    public GetFilteredFavoriteExpensesRequestHandler(
        IAuthRepository authRepository,
        IEntityValidator<ExpensesDataContext> entityValidator,
        IReadGenericRepository<Guid, UserProject, ExpensesDataContext> userProjectRepository,
        IGetFilteredResultStrategy<GetFilteredFavoriteExpensesRequest, FavoriteExpenseResponse> strategy
    ): base(authRepository, entityValidator, userProjectRepository)
    {
        this.strategy = strategy;
    }

    public async Task<FilteredListResponse<FavoriteExpenseResponse>> Handle(GetFilteredFavoriteExpensesRequest request, CancellationToken cancellationToken)
    {
        await this.CheckUserProjectByIdAsync(request.UserProjectId, cancellationToken);
        
        request.CheckBaseFilter();
        request.CategoryIds.CheckIds();
        
        return await this.strategy.GetFilteredResultAsync(request, cancellationToken);
    }
}