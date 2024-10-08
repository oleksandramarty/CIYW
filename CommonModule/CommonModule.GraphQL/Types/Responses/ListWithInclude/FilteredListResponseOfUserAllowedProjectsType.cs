using CommonModule.GraphQL.Types.Common;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.ListWithInclude;

public class FilteredListResponseOfUserAllowedProjectsType: ObjectGraphType<FilteredListResponse<UserAllowedProjectResponse>>
{
    public FilteredListResponseOfUserAllowedProjectsType()
    {
        Field<ListGraphType<ExpenseResponseType>>("entities", resolve: context => context.Source.Entities);
        Field<PaginatorEntityType>("paginator", resolve: context => context.Source.Paginator);
        Field(x => x.TotalCount);
    }
}