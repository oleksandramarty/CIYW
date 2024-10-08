using CommonModule.GraphQL.Types.Responses.Expenses.Models.Balances;
using CommonModule.Shared.Responses.Expenses.Models.Balances;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Expenses.Models.Projects;

public class UserProjectResponseType : ObjectGraphType<UserProjectResponse>
{
    public UserProjectResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Title);
        Field(x => x.IsActive);
        Field(x => x.CreatedUserId);
        Field<ListGraphType<BalanceResponseType>>(nameof(UserProjectResponse.Balances));
        Field(x => x.Version);
        Field(x => x.Created);
        Field(x => x.Modified, nullable: true);
    }
}