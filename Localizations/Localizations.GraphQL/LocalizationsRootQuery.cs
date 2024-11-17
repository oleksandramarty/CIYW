using CommonModule.GraphQL.QueryResolver;

namespace Localizations.GraphQL;

public class LocalizationsRootQuery : GraphQlQueryHelper
{
    public LocalizationsRootQuery()
    {
        this.AddLocalizationsQueries();
    }
}