using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.Expenses.Expenses;

public class CreateFavoriteExpenseInputType : InputObjectGraphType
{
    public CreateFavoriteExpenseInputType()
    {
        Name = "CreateFavoriteExpenseInput";
        Field<NonNullGraphType<StringGraphType>>("title");
        Field<StringGraphType>("description");
        Field<DecimalGraphType>("limit");
        Field<IntGraphType>("categoryId");
        Field<IntGraphType>("frequencyId");
        Field<NonNullGraphType<IntGraphType>>("currencyId");
        Field<NonNullGraphType<IdGraphType>>("userProjectId");
        Field<NonNullGraphType<IntGraphType>>("iconId");
    }
}