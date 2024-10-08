using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.Expenses.Expenses;

public class UpdateExpenseInputType : InputObjectGraphType
{
    public UpdateExpenseInputType()
    {
        Name = "UpdateExpenseInput";
        Field<NonNullGraphType<StringGraphType>>("title");
        Field<StringGraphType>("description");
        Field<NonNullGraphType<DecimalGraphType>>("amount");
        Field<NonNullGraphType<DateTimeGraphType>>("date");
        Field<NonNullGraphType<IntGraphType>>("categoryId");
        Field<NonNullGraphType<IdGraphType>>("balanceId");
    }
}