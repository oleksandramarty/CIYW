using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.Expenses.Expenses;

public class UpdatePlannedExpenseInputType : InputObjectGraphType
{
    public UpdatePlannedExpenseInputType()
    {
        Name = "UpdatePlannedExpenseInput";
        Field<NonNullGraphType<StringGraphType>>("title");
        Field<StringGraphType>("description");
        Field<NonNullGraphType<DecimalGraphType>>("amount");
        Field<NonNullGraphType<IntGraphType>>("categoryId");
        Field<NonNullGraphType<IdGraphType>>("balanceId");
        Field<NonNullGraphType<DateTimeGraphType>>("startDate");
        Field<DateTimeGraphType>("endDate");
        Field<NonNullGraphType<IntGraphType>>("frequencyId");
        Field<NonNullGraphType<BooleanGraphType>>("isActive");
    }
}