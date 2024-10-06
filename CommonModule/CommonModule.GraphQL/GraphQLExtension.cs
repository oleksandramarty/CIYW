using CommonModule.Core.Exceptions;
using CommonModule.Shared.Common;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Requests.Base;
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
            new QueryArgument<StringGraphType> { Name = "parentClass" },
            new QueryArgument<StringGraphType> { Name = "column" },
            new QueryArgument<StringGraphType> { Name = "direction" });
    }

    public static BaseFilterRequest GetBaseFilterQuery(this IResolveFieldContext<object?> context)
    {
        BaseFilterRequest query = new BaseFilterRequest();
        query.Paginator = new PaginatorEntity(
            context.GetArgument<int?>("pageNumber") ?? 1,
            context.GetArgument<int?>("pageSize") ?? 5,
            context.GetArgument<bool?>("isFull") ?? false);

        query.Sort = new BaseSortableRequest
        {
            Column = context.GetArgument<string?>("column") ?? "Date",
            Direction = context.GetArgument<string?>("direction") ?? "desc"
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

        return query;
    }

    public static List<TId> GetIdsModel<TId>(this IResolveFieldContext<object?> context, string name)
    {
        List<TId?> ids = context.GetArgument<List<TId?>>(name);
        return ids != null && ids.Any(x => x != null)
            ? ids.Where(x => x != null).ToList()
            : null;
    }

    public static void IsAuthenticated(this IResolveFieldContext<object?> context, bool isAuthenticated = true)
    {
        if (context?.User?.Identity?.IsAuthenticated != true && isAuthenticated)
        {
            throw new ForbiddenException();
        }
    }
}