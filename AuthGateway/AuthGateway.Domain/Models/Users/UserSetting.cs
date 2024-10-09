using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;

namespace AuthGateway.Domain.Models.Users;

public class UserSetting: BaseDateTimeEntity<Guid>, IBaseVersionEntity
{
    public string DefaultLocale { get; set; }
    public int TimeZone { get; set; }
    public int CountryId { get; set; }
    public int DefaultUserProjectCurrencyId { get; set; }
    public Guid DefaultUserProjectId { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public string Version { get; set; }
}