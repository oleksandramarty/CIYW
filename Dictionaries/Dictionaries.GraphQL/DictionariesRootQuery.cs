using CommonModule.GraphQL;
using CommonModule.GraphQL.QueryResolver;
using CommonModule.GraphQL.Types.Responses.Dictionaries;
using CommonModule.Shared.Responses.Dictionaries;
using Dictionaries.Mediatr.Mediatr.Requests;

namespace Dictionaries.GraphQL;

public class DictionariesRootQuery: GraphQLQueryResolver
{
    public DictionariesRootQuery()
    {
        this.GetResultForEmptyCommand<
            SiteSettingsResponseType, 
            GetSiteSettingsRequest, 
            SiteSettingsResponse
        >(GraphQLEndpoints.SiteSettings);
    }
}  