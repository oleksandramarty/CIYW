using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.GraphQL;
using CommonModule.GraphQL.QueryResolver;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway;
using CommonModule.GraphQL.Types.Responses.AuthGateway;
using CommonModule.GraphQL.Types.Responses.AuthGateway.Users;
using CommonModule.Shared.Responses.Auth;
using CommonModule.Shared.Responses.AuthGateway.Users;

namespace AuthGateway.GraphQL;

public class AuthGatewayRootQuery: GraphQLQueryResolver
{
    public AuthGatewayRootQuery()
    {
        this.GetResultForEmptyCommand<
            UserResponseType, 
            GetCurrentUserRequest, 
            UserResponse
        >(GraphQLEndpoints.CurrentUser);
        
        this.GetResultForNonEmptyCommand<
            AuthSignInRequestInputType,
            JwtTokenResponseType,
            JwtTokenResponse,
            AuthSignInRequest,
            JwtTokenResponse
        >(GraphQLEndpoints.SignIn);
    }
}