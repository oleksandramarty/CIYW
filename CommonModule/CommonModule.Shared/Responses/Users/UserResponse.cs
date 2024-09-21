using CommonModule.Shared.Common;
using CommonModule.Shared.Enums;

namespace CommonModule.Shared.Responses.Users;

public class UserResponse: BaseDateTimeEntity<Guid>
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
    
    public ICollection<UserRoleResponse>? Roles { get; set; }
}