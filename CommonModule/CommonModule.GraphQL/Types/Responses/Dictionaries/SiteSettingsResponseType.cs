using CommonModule.Shared.Responses.Dictionaries;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Dictionaries;

public class SiteSettingsResponseType : ObjectGraphType<SiteSettingsResponse>
{
    public SiteSettingsResponseType()
    {
        Field(x => x.Locale);
        Field<CacheVersionResponseType>(
            name: "version",
            resolve: context => context.Source.Version
        );
    }
}
    
public class CacheVersionResponseType : ObjectGraphType<CacheVersionResponse>
{
    public CacheVersionResponseType()
    {
        Field(x => x.LocalizationPublic);
        Field(x => x.Localization);
        Field(x => x.Category);
        Field(x => x.Currency);
        Field(x => x.Country);
        Field(x => x.Locale);
        Field(x => x.Frequency);
    }
}