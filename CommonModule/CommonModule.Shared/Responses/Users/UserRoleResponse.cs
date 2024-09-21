using CommonModule.Shared.Common;

namespace CommonModule.Shared.Responses.Users;

public class UserRoleResponse : BaseIdEntity<Guid>
{
    public Guid UserId { get; set; }
    public int RoleId { get; set; }
    public RoleResponse Role { get; set; }
}