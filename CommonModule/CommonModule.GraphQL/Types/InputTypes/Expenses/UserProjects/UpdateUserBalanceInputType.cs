using CommonModule.Shared.Enums.Expenses;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.Expenses.UserProjects;

public class UpdateUserBalanceInputType: InputObjectGraphType
{
    public UpdateUserBalanceInputType()
    {
        Name = "UpdateUserBalanceInput";
        Field<NonNullGraphType<IntGraphType>>("currencyId");
        Field<NonNullGraphType<StringGraphType>>("title");
        Field<NonNullGraphType<IntGraphType>>("balanceTypeId");
        Field<NonNullGraphType<BooleanGraphType>>("isActive");
        Field<NonNullGraphType<GuidGraphType>>("userProjectId");
        Field<NonNullGraphType<IntGraphType>>("iconId");
    }
}