using CommonModule.GraphQL.Types.Common;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.ListWithInclude;

public class FilteredListResponseOfExpensesType : ObjectGraphType<FilteredListResponse<ExpenseResponse>>

{
    public FilteredListResponseOfExpensesType()
    {
        Field<ListGraphType<ExpenseResponseType>>("entities", resolve: context => context.Source.Entities);
        Field<PaginatorEntityType>("paginator", resolve: context => context.Source.Paginator);
        Field(x => x.TotalCount);
    }
}