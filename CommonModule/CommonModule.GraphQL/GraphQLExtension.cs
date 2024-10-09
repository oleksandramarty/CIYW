using CommonModule.Core.Exceptions;
using CommonModule.GraphQL.Types.EnumType;
using CommonModule.Shared.Common;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Requests.Base;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using GraphQL;
using GraphQL.Types;

namespace CommonModule.GraphQL;

public static class GraphQLExtension
{
    public static IEnumerable<QueryArgument> GetPageableQueryArguments()
    {
        return new QueryArguments(new QueryArgument<BooleanGraphType> { Name = "isFull" },
            new QueryArgument<IntGraphType> { Name = "pageNumber" },
            new QueryArgument<IntGraphType> { Name = "pageSize" },
            new QueryArgument<DateTimeGraphType> { Name = "dateFrom" },
            new QueryArgument<DateTimeGraphType> { Name = "dateTo" },
            new QueryArgument<StringGraphType> { Name = "query" },
            new QueryArgument<ColumnEnumType> { Name = "column" },
            new QueryArgument<OrderDirectionEnumType> { Name = "direction" },
            new QueryArgument<DecimalGraphType> { Name = "amountFrom" },
            new QueryArgument<DecimalGraphType> { Name = "amountTo" },
            new QueryArgument<IdGraphType> { Name = "userProjectId" },
            new QueryArgument<ListGraphType<IntGraphType>> { Name = "categoryIds" }
        );
    }

    public static TFilter GetFilterQuery<TFilter>(this IResolveFieldContext<object?> context)
        where TFilter: IBaseFilterRequest, new ()
    {
        TFilter query = new TFilter();
        query.Paginator = new PaginatorEntity(
            context.GetArgument<int?>("pageNumber") ?? 1,
            context.GetArgument<int?>("pageSize") ?? 5,
            context.GetArgument<bool?>("isFull") ?? false);

        query.Sort = new BaseSortableRequest
        {
            Column = context.GetArgument<ColumnEnum?>("column") ?? ColumnEnum.Created,
            Direction = context.GetArgument<OrderDirectionEnum?>("direction") ?? OrderDirectionEnum.Desc
        };

        var startDate = context.GetArgument<DateTime?>("dateFrom");
        var endDate = context.GetArgument<DateTime?>("dateTo");

        if (startDate.HasValue || endDate.HasValue)
        {
            query.DateRange = new BaseDateRangeFilterRequest
            {
                StartDate = startDate,
                EndDate = endDate
            };
        }
        else
        {
            query.DateRange = null;
        }

        var amountFrom = context.GetArgument<decimal?>("amountFrom");
        var amountTo = context.GetArgument<decimal?>("amountTo");

        if (amountFrom.HasValue || amountTo.HasValue)
        {
            query.AmountRange = new BaseAmountRangeFilterRequest()
            {
                AmountFrom = amountFrom,
                AmountTo = amountTo
            };
        }
        else
        {
            query.AmountRange = null;
        }

        query.Query = context.GetArgument<string?>("query");
        
        if (query is GetFilteredExpensesRequest expensesRequest)
        {
            var ids = context.GetIdsModel<int>("categoryIds");
            expensesRequest.UserProjectId = context.GetArgument<Guid>("userProjectId");
            expensesRequest.CategoryIds = new BaseFilterIdsRequest<int>
            {
                Ids = ids
            };

            return (TFilter)(object)expensesRequest;
        }
        
        if (query is GetFilteredPlannedExpensesRequest plannedExpensesRequest)
        {
            var ids = context.GetIdsModel<int>("categoryIds");
            plannedExpensesRequest.UserProjectId = context.GetArgument<Guid>("userProjectId");
            plannedExpensesRequest.CategoryIds = new BaseFilterIdsRequest<int>
            {
                Ids = ids
            };

            return (TFilter)(object)plannedExpensesRequest;
        }

        return query;
    }

    public static void IsAuthenticated(this IResolveFieldContext<object?> context, bool isAuthenticated = true)
    {
        if (context?.User?.Identity?.IsAuthenticated != true && isAuthenticated)
        {
            throw new ForbiddenException();
        }
    }
    
    private static List<TId> GetIdsModel<TId>(this IResolveFieldContext<object?> context, string name)
    {
        List<TId?> ids = context.GetArgument<List<TId?>>(name);
        return ids != null && ids.Any(x => x != null)
            ? ids.Where(x => x != null).ToList()
            : null;
    }
}