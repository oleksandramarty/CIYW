using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Expenses.Models.Expenses;

public class PlannedExpenseResponseType : ObjectGraphType<PlannedExpenseResponse>
{
    public PlannedExpenseResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Created);
        Field(x => x.Modified, nullable: true);
        Field(x => x.Title, nullable: true);
        Field(x => x.Description, nullable: true);
        Field(x => x.Amount);
        Field(x => x.CategoryId);
        Field(x => x.BalanceId);
        Field(x => x.StartDate);
        Field(x => x.EndDate, nullable: true);
        Field(x => x.UserId);
        Field(x => x.UserProjectId);
        Field(x => x.FrequencyId);
        Field(x => x.IsActive);
        Field(x => x.Version);
    }
}