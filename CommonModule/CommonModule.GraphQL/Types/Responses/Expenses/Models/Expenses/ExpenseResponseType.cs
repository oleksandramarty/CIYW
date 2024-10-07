using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Expenses.Models.Expenses;

public class ExpenseResponseType : ObjectGraphType<ExpenseResponse>
{
    public ExpenseResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Created);
        Field(x => x.Modified, nullable: true);
        Field(x => x.Title);
        Field(x => x.Description, nullable: true);
        Field(x => x.Amount);
        Field(x => x.BalanceBefore);
        Field(x => x.BalanceAfter);
        Field(x => x.BalanceId);
        Field(x => x.Date);
        Field(x => x.CategoryId);
        Field(x => x.UserProjectId);
        Field(x => x.CreatedUserId);
        Field(x => x.Version);
    }
}