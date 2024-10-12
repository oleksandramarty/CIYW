using CommonModule.Shared.Enums.Expenses;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.Expenses.UserProjects;

public class UpdateUserProjectBalancesInputType : InputObjectGraphType
{
    public UpdateUserProjectBalancesInputType()
    {
        Name = "UpdateUserProjectBalancesInput";
        Field<NonNullGraphType<GuidGraphType>>("userProjectId");
        Field<ListGraphType<BalanceInputType>>("balances");
    }
}

public class BalanceInputType : InputObjectGraphType<BalanceDto>
{
    public BalanceInputType()
    {
        Name = "BalanceInput";
        Field(x => x.Id);
        Field(x => x.CurrencyId);
        Field(x => x.Title);
        Field<EnumerationGraphType<BalanceEnum>>("balanceType");
    }
}