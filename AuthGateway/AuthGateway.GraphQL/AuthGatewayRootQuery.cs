using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.GraphQL;
using CommonModule.GraphQL.QueryResolver;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway;
using CommonModule.GraphQL.Types.Responses.AuthGateway;
using CommonModule.GraphQL.Types.Responses.AuthGateway.Users;
using CommonModule.Shared;
using CommonModule.Shared.Responses.Auth;
using CommonModule.Shared.Responses.AuthGateway.Users;

namespace AuthGateway.GraphQL;

public class AuthGatewayRootQuery : GraphQLQueryHelper
{
    public AuthGatewayRootQuery()
    {
        this.AddAuthGatewayQueries();
    }
}