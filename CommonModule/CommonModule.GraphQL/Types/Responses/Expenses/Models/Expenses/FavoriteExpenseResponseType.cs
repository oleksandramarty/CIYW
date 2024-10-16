using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Expenses.Models.Expenses;

public class FavoriteExpenseResponseType : ObjectGraphType<FavoriteExpenseResponse>
{
    public FavoriteExpenseResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Created);
        Field(x => x.Modified, nullable: true);
        Field(x => x.Title);
        Field(x => x.Description, nullable: true);
        Field(x => x.Limit, nullable: true);
        Field(x => x.CurrentAmount, nullable: true);
        Field(x => x.CategoryId, nullable: true);
        Field(x => x.FrequencyId, nullable: true);
        Field(x => x.CurrencyId, nullable: true);
        Field(x => x.EndDate, nullable: true);
        Field(x => x.UserProjectId);
        Field(x => x.IconId);
        Field(x => x.CreatedUserId);
        Field(x => x.Version);
    }
}