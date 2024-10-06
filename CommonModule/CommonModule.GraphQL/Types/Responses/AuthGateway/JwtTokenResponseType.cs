using CommonModule.Shared.Responses.Auth;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.AuthGateway;

public class JwtTokenResponseType : ObjectGraphType<JwtTokenResponse>
{
    public JwtTokenResponseType()
    {
        Field(x => x.Token);
    }
}