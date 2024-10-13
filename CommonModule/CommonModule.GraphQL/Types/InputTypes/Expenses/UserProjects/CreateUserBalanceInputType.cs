using CommonModule.Shared.Enums.Expenses;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.Expenses.UserProjects;

public class CreateUserBalanceInputType: InputObjectGraphType
{
    public CreateUserBalanceInputType()
    {
        Name = "CreateUserBalanceInput";
        Field<NonNullGraphType<IntGraphType>>("currencyId");
        Field<NonNullGraphType<StringGraphType>>("title");
        Field<NonNullGraphType<IntGraphType>>("balanceTypeId");
        Field<NonNullGraphType<BooleanGraphType>>("isActive");
        Field<NonNullGraphType<GuidGraphType>>("userProjectId");
    }
}