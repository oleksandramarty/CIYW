using AutoMapper;
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

public class GetFilteredResultOfExpenseStrategy: IGetFilteredResultStrategy<GetFilteredExpensesRequest, ExpenseResponse>
{
    private readonly IMapper mapper;
    private readonly IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository;

    public GetFilteredResultOfExpenseStrategy(
        IMapper mapper,
        IReadGenericRepository<Guid, Expense, ExpensesDataContext> expenseRepository
        )
    {
        this.mapper = mapper;
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

        var total = await query.CountAsync(cancellationToken);

        List<Expense> entities = new List<Expense>();

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
                case ColumnEnum.Created:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Created) : query.OrderByDescending(x => x.Created);
                    break;
                case ColumnEnum.Modified:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Modified) : query.OrderByDescending(x => x.Modified);
                    break;
                default:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
                    break;
            }
        }

        if (request.Paginator != null)
        {
            entities = await query
                .Skip((request.Paginator.PageNumber - 1) * request.Paginator.PageSize)
                .Take(request.Paginator.PageSize)
                .ToListAsync(cancellationToken);
        }
        else
        {
            entities = await query.ToListAsync(cancellationToken);
        }
        
        return new FilteredListResponse<ExpenseResponse>
        {
            Entities = entities.Select(x => this.mapper.Map<Expense, ExpenseResponse>(x)).ToList(),
            Paginator = request?.Paginator,
            TotalCount = total
        };
    }
}