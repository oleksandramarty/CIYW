using CommonModule.GraphQL.QueryResolver;

namespace AuthGateway.GraphQL;

public class AuthGatewayRootQuery : GraphQLQueryHelper
{
    public AuthGatewayRootQuery()
    {
        this.AddAuthGatewayQueries();
    }
}