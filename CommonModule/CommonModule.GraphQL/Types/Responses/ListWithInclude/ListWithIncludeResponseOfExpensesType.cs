using CommonModule.GraphQL.Types.Common;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.ListWithInclude;

public class ListWithIncludeResponseOfExpensesType : ObjectGraphType<ListWithIncludeResponse<ExpenseResponse>>
{
    public ListWithIncludeResponseOfExpensesType()
    {
        Field<ListGraphType<ExpenseResponseType>>("entities", resolve: context => context.Source.Entities);
        Field<PaginatorEntityType>("paginator", resolve: context => context.Source.Paginator);
        Field(x => x.TotalCount);
        Field(x => x.TotalCountWithoutFilter);
    }
}