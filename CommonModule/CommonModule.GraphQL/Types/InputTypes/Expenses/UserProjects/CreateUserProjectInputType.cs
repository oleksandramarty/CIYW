using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.Expenses.UserProjects;

public class CreateUserProjectInputType : InputObjectGraphType
{
    public CreateUserProjectInputType()
    {
        Name = "CreateUserProjectInput";
        Field<NonNullGraphType<StringGraphType>>("title");
        Field<NonNullGraphType<BooleanGraphType>>("isActive");
        Field<ListGraphType<NonNullGraphType<IntGraphType>>>("currencyIds");
    }
}