using AutoMapper;
using CommonModule.Core.Extensions;
using CommonModule.Core.Strategies.GetFilteredResult;
using CommonModule.Interfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Mediatr.Strategies.GetFilteredResult;

public class GetFilteredResultOfPlannedExpenseStrategy: GetFilteredResultStrategyResponseContext<GetFilteredPlannedExpensesRequest, PlannedExpense, PlannedExpenseResponse>, IGetFilteredResultStrategy<GetFilteredPlannedExpensesRequest, PlannedExpenseResponse>
{
    private readonly IReadGenericRepository<Guid, PlannedExpense, ExpensesDataContext> plannedExpenseRepository;

    public GetFilteredResultOfPlannedExpenseStrategy(
        IMapper mapper,
        IReadGenericRepository<Guid, PlannedExpense, ExpensesDataContext> plannedExpenseRepository
        ): base(mapper)
    {
        this.plannedExpenseRepository = plannedExpenseRepository;
    }

    public async Task<FilteredListResponse<PlannedExpenseResponse>> GetFilteredResultAsync(GetFilteredPlannedExpensesRequest request, CancellationToken cancellationToken)
    {
        var query = this.plannedExpenseRepository.GetQueryable(
            e => e.UserProjectId == request.UserProjectId &&
                 (string.IsNullOrEmpty(request.Query) || EF.Functions.Like(e.Title, $"%{request.Query}%")) &&
                 (!request.CategoryIds.Ids.Any() || request.CategoryIds.Ids.Contains(e.CategoryId)) &&
                 (request.DateRange == null || (
                         request.DateRange.StartDate.HasValue && !request.DateRange.EndDate.HasValue && e.NextDate >= request.DateRange.StartDate ||
                         !request.DateRange.StartDate.HasValue && request.DateRange.EndDate.HasValue && e.NextDate <= request.DateRange.EndDate ||
                         request.DateRange.StartDate.HasValue && request.DateRange.EndDate.HasValue && e.NextDate >= request.DateRange.StartDate && e.NextDate <= request.DateRange.EndDate
                         )
                     ) &&
                 (
                     request.AmountRange == null || (
                         request.AmountRange.AmountFrom.HasValue && !request.AmountRange.AmountTo.HasValue && e.Amount >= request.AmountRange.AmountFrom ||
                         !request.AmountRange.AmountFrom.HasValue && request.AmountRange.AmountTo.HasValue && e.Amount <= request.AmountRange.AmountTo ||
                         request.AmountRange.AmountFrom.HasValue && request.AmountRange.AmountTo.HasValue && e.Amount >= request.AmountRange.AmountFrom && e.Amount <= request.AmountRange.AmountTo
                         )
                     )
            );

        if (request.Sort != null && request.Sort.Column.HasValue)
        {
            switch (request.Sort.Column.Value)
            {
                case ColumnEnum.Title:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Title) : query.OrderByDescending(x => x.Title);
                    break;
                case ColumnEnum.Amount:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Amount) : query.OrderByDescending(x => x.Amount);
                    break;
                case ColumnEnum.CreatedAt:
                    query.SortByCreatedAt(request.Sort.Direction);
                    break;
                case ColumnEnum.UpdatedAt:
                    query.SortByUpdatedAt(request.Sort.Direction);
                    break;
                default:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.NextDate) : query.OrderByDescending(x => x.NextDate);
                    break;
            }
        }
        
        return await this.GetFilteredResultAsync(request, query, cancellationToken);
    }
}