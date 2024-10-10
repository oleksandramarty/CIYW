using CommonModule.GraphQL;
using CommonModule.GraphQL.QueryResolver;
using CommonModule.GraphQL.Types.Responses.Lists;
using CommonModule.GraphQL.Types.Responses.Localizations.Models.Locales;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
using Localizations.Mediatr.Mediatr.Locations.Requests;

namespace Localizations.GraphQL;

public class LocalizationsRootQuery : GraphQLQueryHelper
{
    public LocalizationsRootQuery()
    {
        this.AddLocalizationsQueries();
    }
}