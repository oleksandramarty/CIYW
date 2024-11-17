using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Core;

namespace Expenses.Domain.Models.Projects;

public class UserAllowedProjectEntity: BaseIdEntity<Guid>, IBaseVersionEntity
{
    public Guid UserProjectId { get; set; }
    public UserProjectEntity? UserProject { get; set; }
    
    public Guid UserId { get; set; }
    
    public bool IsReadOnly { get; set; }
    [Required]
    [StringLength(32, MinimumLength = 32)]
    public string Version { get; set; } = VersionExtension.GenerateVersion();
}