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
        Field(x => x.LocalizationPublic, nullable: true);
        Field(x => x.Localization, nullable: true);
        Field(x => x.Category, nullable: true);
        Field(x => x.Currency, nullable: true);
        Field(x => x.Country, nullable: true);
        Field(x => x.Locale, nullable: true);
        Field(x => x.Frequency, nullable: true);
        Field(x => x.BalanceType, nullable: true);
        Field(x => x.IconCategory, nullable: true);
    }
}