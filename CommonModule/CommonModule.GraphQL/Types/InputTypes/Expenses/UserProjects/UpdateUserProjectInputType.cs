using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.Expenses.UserProjects;

public class UpdateUserProjectInputType : InputObjectGraphType
{
    public UpdateUserProjectInputType()
    {
        Name = "UpdateUserProjectInput";
        Field<NonNullGraphType<StringGraphType>>("title");
        Field<NonNullGraphType<BooleanGraphType>>("isActive");
    }
}