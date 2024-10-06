using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.AuthGateway;

public class AuthSignInRequestInputType : InputObjectGraphType
{
    public AuthSignInRequestInputType()
    {
        Name = "AuthSignInRequestInputType";
        Field<NonNullGraphType<StringGraphType>>("login");
        Field<NonNullGraphType<StringGraphType>>("password");
        Field<NonNullGraphType<BooleanGraphType>>("rememberMe");
    }
}