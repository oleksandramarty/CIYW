using CommonModule.GraphQL.QueryResolver;

namespace Localizations.GraphQL;

public class LocalizationsRootQuery : GraphQLQueryHelper
{
    public LocalizationsRootQuery()
    {
        this.AddLocalizationsQueries();
    }
}