using CommonModule.Shared.Responses.Expenses.Models.Projects;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Expenses.Models.Projects;

public class ListUserProjectResponseType: ObjectGraphType<List<UserProjectResponse>>
{
    public ListUserProjectResponseType()
    {
        Field<ListGraphType<UserProjectResponseType>>(
            name: "items",
            resolve: context => context.Source
        );
    }
}