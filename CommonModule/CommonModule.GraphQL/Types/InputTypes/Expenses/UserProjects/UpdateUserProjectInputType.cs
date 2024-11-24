using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.Expenses.UserProjects;

public sealed class UpdateUserProjectInputType : InputObjectGraphType
{
    public UpdateUserProjectInputType()
    {
        Name = "UpdateUserProjectInput";
        Field<NonNullGraphType<StringGraphType>>("title");
    }
}