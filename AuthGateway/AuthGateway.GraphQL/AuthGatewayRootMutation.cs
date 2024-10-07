using CommonModule.GraphQL.MutationResolver;

namespace AuthGateway.GraphQL;

public class AuthGatewayRootMutation: GraphQLMutationResolver
{
    public AuthGatewayRootMutation()
    {
        Name = "Mutation";
    }
}