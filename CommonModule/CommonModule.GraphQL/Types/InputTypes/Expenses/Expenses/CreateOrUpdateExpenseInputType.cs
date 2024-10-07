using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.Expenses.Expenses;

public class CreateOrUpdateExpenseInputType : InputObjectGraphType
{
    public CreateOrUpdateExpenseInputType()
    {
        Name = "CreateOrUpdateExpenseInput";
        Field<NonNullGraphType<StringGraphType>>("title");
        Field<StringGraphType>("description");
        Field<NonNullGraphType<DecimalGraphType>>("amount");
        Field<NonNullGraphType<DateTimeGraphType>>("date");
        Field<NonNullGraphType<IntGraphType>>("categoryId");
        Field<NonNullGraphType<IdGraphType>>("userProjectId");
        Field<NonNullGraphType<IdGraphType>>("balanceId");
    }
}