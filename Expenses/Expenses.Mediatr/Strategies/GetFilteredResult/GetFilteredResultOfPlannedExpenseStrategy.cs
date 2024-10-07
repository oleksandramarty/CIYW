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

public class GetFilteredResultOfPlannedExpenseStrategy: IGetFilteredResultStrategy<GetFilteredPlannedExpensesRequest, PlannedExpenseResponse>
{
    private readonly IMapper mapper;
    private readonly IReadGenericRepository<Guid, PlannedExpense, ExpensesDataContext> plannedExpenseRepository;

    public GetFilteredResultOfPlannedExpenseStrategy(
        IMapper mapper,
        IReadGenericRepository<Guid, PlannedExpense, ExpensesDataContext> plannedExpenseRepository
        )
    {
        this.mapper = mapper;
        this.plannedExpenseRepository = plannedExpenseRepository;
    }

    public async Task<ListWithIncludeResponse<PlannedExpenseResponse>> GetFilteredResultAsync(GetFilteredPlannedExpensesRequest request, CancellationToken cancellationToken)
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

        var total = await query.CountAsync(cancellationToken);

        List<PlannedExpense> entities = new List<PlannedExpense>();

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
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.NextDate) : query.OrderByDescending(x => x.NextDate);
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
        
        return new ListWithIncludeResponse<PlannedExpenseResponse>
        {
            Entities = entities.Select(x => this.mapper.Map<PlannedExpense, PlannedExpenseResponse>(x)).ToList(),
            Paginator = request?.Paginator,
            TotalCount = total
        };
    }
}