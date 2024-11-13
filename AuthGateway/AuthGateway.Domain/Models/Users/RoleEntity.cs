using CommonModule.Shared.Common;
using CommonModule.Shared.Enums;

namespace AuthGateway.Domain.Models.Users;

public class RoleEntity: BaseIdEntity<int>
{
    public string Title { get; set; }
    public UserRoleEnum UserRole { get; set; }
    
    public ICollection<UserRoleEntity> Users { get; set; }
}