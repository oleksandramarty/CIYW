using CommonModule.Shared.Responses.Expenses.Models.Balances;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Expenses.Models.Balances
{
    public class BalanceResponseType : ObjectGraphType<BalanceResponse>
    {
        public BalanceResponseType()
        {
            Field(x => x.Id);
            Field(x => x.Created);
            Field(x => x.Modified, nullable: true);
            Field(x => x.UserId);
            Field(x => x.Amount);
            Field(x => x.CurrencyId);
            Field(x => x.UserProjectId);
            Field(x => x.Version);
        }
    }
}