using CommonModule.Shared.Responses.AuthGateway.Users;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.AuthGateway.Users;

public class UserSettingResponseType : ObjectGraphType<UserSettingResponse>
{
    public UserSettingResponseType()
    {
        Field(x => x.Id);
        Field(x => x.DefaultLocale);
        Field(x => x.TimeZone);
        Field(x => x.CurrencyId, nullable: true);
        Field(x => x.CountryId, nullable: true);
        Field(x => x.DefaultUserProject);
        Field(x => x.UserId);
        Field(x => x.Version);
    }
}