using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;

namespace CommonModule.Shared.Responses.AuthGateway.Users;

public class UserResponse: BaseDateTimeEntity<Guid>, IActivatable
{
    public string Login { get; set; }
    public string LoginNormalized { get; set; }
    public string Email { get; set; }
    public string EmailNormalized { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public bool IsActive { get; set; }
    public bool IsTemporaryPassword { get; set; }
    public int? CountryId { get; set; }
    public int? CurrencyId { get; set; }
    public UserAuthMethodEnum AuthType { get; set; }
    
    public DateTime? LastForgotPassword { get; set; }
    public DateTime? LastForgotPasswordRequest { get; set; }
    
    public ICollection<RoleResponse> Roles { get; set; }
    
    public UserSettingResponse UserSetting { get; set; }
}