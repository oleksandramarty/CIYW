using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using CommonModule.GraphQL;
using CommonModule.GraphQL.MutationResolver;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway.Users;
using CommonModule.Shared;
using GraphQL.Types;

namespace AuthGateway.GraphQL;

public class AuthGatewayRootMutation: GraphQLMutationHelper
{
    public AuthGatewayRootMutation()
    {
        Name = "Mutation";
        this.AddAuthGatewayMutations();
    }
}