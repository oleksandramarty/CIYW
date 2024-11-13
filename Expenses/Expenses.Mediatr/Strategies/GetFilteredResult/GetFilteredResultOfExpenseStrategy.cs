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

public class GetFilteredResultOfExpenseStrategy: GetFilteredResultStrategyResponseContext<GetFilteredExpensesRequest, ExpenseEntity, ExpenseResponse>, IGetFilteredResultStrategy<GetFilteredExpensesRequest, ExpenseResponse>
{
    private readonly IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> expenseRepository;

    public GetFilteredResultOfExpenseStrategy(
        IMapper mapper,
        IReadGenericRepository<Guid, ExpenseEntity, ExpensesDataContext> expenseRepository
        ): base(mapper)
    {
        this.expenseRepository = expenseRepository;
    }

    public async Task<FilteredListResponse<ExpenseResponse>> GetFilteredResultAsync(GetFilteredExpensesRequest request, CancellationToken cancellationToken)
    {
        var query = this.expenseRepository.GetQueryable(
            e => e.UserProjectId == request.UserProjectId &&
                 (string.IsNullOrEmpty(request.Query) || EF.Functions.Like(e.Title, $"%{request.Query}%")) &&
                 (!request.CategoryIds.Ids.Any() || request.CategoryIds.Ids.Contains(e.CategoryId)) &&
                 (request.DateRange == null || (
                         request.DateRange.StartDate.HasValue && !request.DateRange.EndDate.HasValue && e.Date >= request.DateRange.StartDate ||
                         !request.DateRange.StartDate.HasValue && request.DateRange.EndDate.HasValue && e.Date <= request.DateRange.EndDate ||
                         request.DateRange.StartDate.HasValue && request.DateRange.EndDate.HasValue && e.Date >= request.DateRange.StartDate && e.Date <= request.DateRange.EndDate
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
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
                    break;
            }
        }
        
        return await this.GetFilteredResultAsync(request, query, cancellationToken);
    }
}