using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace Expenses.Domain.Models.Projects;

public class UserAllowedProject: BaseIdEntity<Guid>, IBaseVersionEntity
{
    public Guid UserProjectId { get; set; }
    public UserProject UserProject { get; set; }
    
    public Guid UserId { get; set; }
    
    public bool IsReadOnly { get; set; }
    public string Version { get; set; }
}