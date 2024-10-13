using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Countries;

public class CountryResponseType : ObjectGraphType<CountryResponse>
{
    public CountryResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Title, nullable: true);
        Field(x => x.Code);
        Field(x => x.TitleEn);
        Field(x => x.IsActive);
    }
}