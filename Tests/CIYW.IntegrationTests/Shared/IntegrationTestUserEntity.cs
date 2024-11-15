using AuthGateway.Domain.Models.Users;
using CommonModule.Shared.Enums;
using Expenses.Domain.Models.Projects;

namespace CIYW.IntegrationTests.Shared;

public class IntegrationTestUserEntity
{
    public UserEntity? User { get; set; }
    public List<UserProjectEntity> UserProjects { get; set; }
    public UserRoleEnum? Role { get; set; }
}