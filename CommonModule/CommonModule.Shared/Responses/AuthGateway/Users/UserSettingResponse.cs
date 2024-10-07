using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Responses.AuthGateway.Users;

public class UserSettingResponse: BaseIdEntity<Guid>, IBaseVersionEntity
{
    public string DefaultLocale { get; set; }
    public int TimeZone { get; set; }
    public int? CurrencyId { get; set; }
    public int? CountryId { get; set; }
    public Guid DefaultUserProject { get; set; }
    public Guid UserId { get; set; }
    
    public string Version { get; set; }
}