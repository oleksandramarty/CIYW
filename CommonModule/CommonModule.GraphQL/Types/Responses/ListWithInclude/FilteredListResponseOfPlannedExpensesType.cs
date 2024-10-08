using CommonModule.GraphQL.Types.Common;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Base;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.ListWithInclude
{
    public class FilteredListResponseOfPlannedExpensesType : ObjectGraphType<FilteredListResponse<PlannedExpenseResponse>>
    {
        public FilteredListResponseOfPlannedExpensesType()
        {
            Field<ListGraphType<PlannedExpenseResponseType>>("entities", resolve: context => context.Source.Entities);
            Field<PaginatorEntityType>("paginator", resolve: context => context.Source.Paginator);
            Field(x => x.TotalCount);
        }
    }
}