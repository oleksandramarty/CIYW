using CommonModule.GraphQL.MutationResolver;

namespace AuthGateway.GraphQL;

public class AuthGatewayRootMutation: GraphQLMutationHelper
{
    public AuthGatewayRootMutation()
    {
        Name = "Mutation";
        this.AddAuthGatewayMutations();
    }
}