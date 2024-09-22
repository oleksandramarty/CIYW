using CommonModule.Shared.Common;

namespace Expenses.Domain.Models.Projects;

public class UserAllowedProject: BaseIdEntity<Guid>
{
    public Guid UserProjectId { get; set; }
    public UserProject UserProject { get; set; }
    
    public Guid UserId { get; set; }
    
    public bool IsReadOnly { get; set; }
}