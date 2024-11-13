using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace Expenses.Domain.Models.Projects;

public class UserAllowedProjectEntity: BaseIdEntity<Guid>, IBaseVersionEntity
{
    public Guid UserProjectId { get; set; }
    public UserProjectEntity UserProject { get; set; }
    
    public Guid UserId { get; set; }
    
    public bool IsReadOnly { get; set; }
    public string Version { get; set; }
}