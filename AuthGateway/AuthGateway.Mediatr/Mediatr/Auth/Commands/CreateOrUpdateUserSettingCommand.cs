using CommonModule.Shared.Common;
using MediatR;

namespace AuthGateway.Mediatr.Mediatr.Auth.Commands;

public class CreateOrUpdateUserSettingCommand: BaseIdEntity<Guid?>, IRequest
{
    public string DefaultLocale { get; set; }
    public int TimeZone { get; set; }
    public int CurrencyId { get; set; }
    public int CountryId { get; set; }
    public Guid DefaultUserProject { get; set; }
}