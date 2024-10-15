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

public class GetFilteredResultOfFavoriteExpenseStrategy: IGetFilteredResultStrategy<GetFilteredFavoriteExpensesRequest, FavoriteExpenseResponse>
{
    private readonly IMapper mapper;
    private readonly IReadGenericRepository<Guid, FavoriteExpense, ExpensesDataContext> favoriteExpenseRepository;

    public GetFilteredResultOfFavoriteExpenseStrategy(
        IMapper mapper,
        IReadGenericRepository<Guid, FavoriteExpense, ExpensesDataContext> favoriteExpenseRepository
        )
    {
        this.mapper = mapper;
        this.favoriteExpenseRepository = favoriteExpenseRepository;
    }

    public async Task<FilteredListResponse<FavoriteExpenseResponse>> GetFilteredResultAsync(GetFilteredFavoriteExpensesRequest request, CancellationToken cancellationToken)
    {
        var query = this.favoriteExpenseRepository.GetQueryable(
            e => e.UserProjectId == request.UserProjectId &&
                 (string.IsNullOrEmpty(request.Query) || EF.Functions.Like(e.Title, $"%{request.Query}%")) &&
                 (!request.CategoryIds.Ids.Any() || request.CategoryIds.Ids.Contains(e.CategoryId)) &&
                 (request.DateRange == null || (
                         request.DateRange.StartDate.HasValue && !request.DateRange.EndDate.HasValue && e.EndDate >= request.DateRange.StartDate && e.EndDate.HasValue ||
                         !request.DateRange.StartDate.HasValue && request.DateRange.EndDate.HasValue && e.EndDate <= request.DateRange.EndDate && e.EndDate.HasValue ||
                         request.DateRange.StartDate.HasValue && request.DateRange.EndDate.HasValue && e.EndDate >= request.DateRange.StartDate && e.EndDate <= request.DateRange.EndDate && e.EndDate.HasValue
                         )
                     ) &&
                 (
                     request.AmountRange == null || (
                         request.AmountRange.AmountFrom.HasValue && !request.AmountRange.AmountTo.HasValue && e.Limit >= request.AmountRange.AmountFrom ||
                         !request.AmountRange.AmountFrom.HasValue && request.AmountRange.AmountTo.HasValue && e.Limit <= request.AmountRange.AmountTo ||
                         request.AmountRange.AmountFrom.HasValue && request.AmountRange.AmountTo.HasValue && e.Limit >= request.AmountRange.AmountFrom && e.Limit <= request.AmountRange.AmountTo
                         )
                     )
            );

        var total = await query.CountAsync(cancellationToken);

        List<FavoriteExpense> entities = new List<FavoriteExpense>();

        if (request.Sort != null && request.Sort.Column.HasValue)
        {
            switch (request.Sort.Column.Value)
            {
                case ColumnEnum.Title:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Title) : query.OrderByDescending(x => x.Title);
                    break;
                case ColumnEnum.Amount:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Limit) : query.OrderByDescending(x => x.Limit);
                    break;
                case ColumnEnum.CurrentAmount:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.CurrentAmount) : query.OrderByDescending(x => x.CurrentAmount);
                    break;
                case ColumnEnum.Created:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Created) : query.OrderByDescending(x => x.Created);
                    break;
                case ColumnEnum.Modified:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.Modified) : query.OrderByDescending(x => x.Modified);
                    break;
                default:
                    query = request.Sort.Direction == OrderDirectionEnum.Asc ? query.OrderBy(x => x.EndDate) : query.OrderByDescending(x => x.EndDate);
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
        
        return new FilteredListResponse<FavoriteExpenseResponse>
        {
            Entities = entities.Select(x => this.mapper.Map<FavoriteExpense, FavoriteExpenseResponse>(x)).ToList(),
            Paginator = request?.Paginator,
            TotalCount = total
        };
    }
}