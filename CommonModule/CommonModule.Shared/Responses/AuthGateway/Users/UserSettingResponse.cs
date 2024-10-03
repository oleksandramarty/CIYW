using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Responses.AuthGateway.Users;

public class UserSettingResponse: BaseIdEntity<Guid>, IBaseVersionEntity
{
    public string DefaultLocale { get; set; }
    public int DefaultTimeZone { get; set; }
    public int DefaultCurrency { get; set; }
    public Guid DefaultUserProject { get; set; }
    public Guid UserId { get; set; }
    public string Version { get; set; }
}