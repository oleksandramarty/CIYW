using CommonModule.Shared.Common;

namespace AuthGateway.Domain.Models.Users;

public class UserRole: BaseIdEntity<Guid>
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
}