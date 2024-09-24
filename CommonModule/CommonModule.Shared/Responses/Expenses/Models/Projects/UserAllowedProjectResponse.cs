using CommonModule.Shared.Common;

namespace CommonModule.Shared.Responses.Expenses.Models.Projects;

public class UserAllowedProjectResponse: BaseIdEntity<Guid>
{
    public Guid UserProjectId { get; set; }
    public UserProjectResponse UserProject { get; set; }
    
    public Guid UserId { get; set; }
    
    public bool IsReadOnly { get; set; }
}