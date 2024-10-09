using CommonModule.GraphQL.Types.EnumType;
using CommonModule.Shared.Enums;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.InputTypes.AuthGateway;

public class AuthSignUpInputType : InputObjectGraphType
{
    public AuthSignUpInputType()
    {
        Name = "AuthSignUpCommandInputType";
        Field<NonNullGraphType<StringGraphType>>("login");
        Field<NonNullGraphType<StringGraphType>>("email");
        Field<NonNullGraphType<StringGraphType>>("password");
        Field<NonNullGraphType<StringGraphType>>("passwordAgain");
        Field<NonNullGraphType<UserRoleEnumType>>("role");
    }
}