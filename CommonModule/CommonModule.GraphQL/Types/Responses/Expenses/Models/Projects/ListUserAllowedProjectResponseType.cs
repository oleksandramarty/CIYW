using CommonModule.Shared.Responses.Expenses.Models.Projects;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Expenses.Models.Projects;

public class ListUserAllowedProjectResponseType : ObjectGraphType<List<UserAllowedProjectResponse>>
{
    public ListUserAllowedProjectResponseType()
    {
        Field<ListGraphType<UserAllowedProjectResponseType>>(
            name: "items",
            resolve: context => context.Source
        );
    }
}