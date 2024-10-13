using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Currencies;

public class CurrencyResponseType : ObjectGraphType<CurrencyResponse>
{
    public CurrencyResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Title, nullable: true);
        Field(x => x.Code);
        Field(x => x.Symbol);
        Field(x => x.TitleEn);
        Field(x => x.IsActive);
    }
}