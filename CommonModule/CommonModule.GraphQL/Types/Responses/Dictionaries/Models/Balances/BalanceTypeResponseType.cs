using CommonModule.Shared.Responses.Dictionaries.Models.Balances;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Balances;

public class BalanceTypeResponseType : ObjectGraphType<BalanceTypeResponse>
{
    public BalanceTypeResponseType()
    {
        Field(x => x.Id).Description("The ID of the balance type.");
        Field(x => x.Title, nullable: true).Description("The title of the balance type.");
        Field(x => x.IsActive).Description("Indicates if the balance type is active.");
        Field(x => x.Icon, nullable: true).Description("The icon of the balance type.");
        Field(x => x.Type).Description("The type of the balance.");
    }
}