using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.AuthGateway.Users;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.AuthGateway.Users;

public class UserResponseType : ObjectGraphType<UserResponse>
{
    public UserResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Login);
        Field(x => x.Email);
        Field(x => x.IsActive);
        Field(x => x.IsTemporaryPassword);
        Field(x => x.AuthType, type: typeof(EnumerationGraphType<UserAuthMethodEnum>));
        Field(x => x.LastForgotPassword, nullable: true);
        Field(x => x.LastForgotPasswordRequest, nullable: true);
        Field<ListGraphType<RoleResponseType>>(nameof(UserResponse.Roles));
        Field(x => x.UserSetting, type: typeof(UserSettingResponseType));
        Field(x => x.Version);
    }
}