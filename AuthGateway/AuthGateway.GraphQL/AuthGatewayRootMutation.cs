using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using CommonModule.GraphQL;
using CommonModule.GraphQL.MutationResolver;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway.Users;
using CommonModule.Shared;
using GraphQL.Types;

namespace AuthGateway.GraphQL;

public class AuthGatewayRootMutation: GraphQLMutationResolver
{
    public AuthGatewayRootMutation()
    {
        Name = "Mutation";
        this.CreateEntity<CreateOrUpdateUserSettingsInputType, CreateUserSettingCommand>(GraphQLEndpoints.CreateUserSettings);
        this.UpdateEntity<CreateOrUpdateUserSettingsInputType, GuidGraphType, Guid, UpdateUserSettingCommand>(GraphQLEndpoints.UpdateUserSettings);

        this.CreateEntity<AuthSignUpInputType, AuthSignUpCommand>(GraphQLEndpoints.SignUp);
    }
}