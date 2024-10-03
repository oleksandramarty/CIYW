using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;

namespace AuthGateway.Domain.Models.Users;

public class User: BaseDateTimeEntity<Guid>, IActivatable, IBaseVersionEntity
{
    public string Login { get; set; }
    public string LoginNormalized { get; set; }
    public string Email { get; set; }
    public string EmailNormalized { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public bool IsActive { get; set; }
    public bool IsTemporaryPassword { get; set; }
    public UserAuthMethodEnum AuthType { get; set; }
    
    public DateTime? LastForgotPassword { get; set; }
    public DateTime? LastForgotPasswordRequest { get; set; }
    
    public ICollection<UserRole> Roles { get; set; }
    
    // TODO make non nullable after migration
    public Guid? UserSettingId { get; set; }
    public UserSetting UserSetting { get; set; }
    public string Version { get; set; }
}