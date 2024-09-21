using CommonModule.Shared.Common;
using CommonModule.Shared.Enums;

namespace CommonModule.Shared.Responses.Users;

public class RoleResponse: BaseIdEntity<int>
{
    public string Title { get; set; }
    public UserRoleEnum UserRole { get; set; }
}