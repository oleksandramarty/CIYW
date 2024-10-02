using CommonModule.Shared.Common;
using CommonModule.Shared.Enums;

namespace AuthGateway.Domain.Models.Users;

public class UserSetting: BaseDateTimeEntity<Guid>
{
    public string DefaultLocale { get; set; }
    public int TimeZone { get; set; }
    public int? CurrencyId { get; set; }
    public int? CountryId { get; set; }
    public Guid DefaultUserProject { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}