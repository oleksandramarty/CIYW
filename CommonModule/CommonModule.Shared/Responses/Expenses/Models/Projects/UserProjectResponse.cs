using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Core;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Expenses.Models.Balances;

namespace CommonModule.Shared.Responses.Expenses.Models.Projects;

public class UserProjectResponse: BaseDateTimeEntity<Guid>, IStatusEntity, IBaseVersionEntity
{
    public string? Title { get; set; }
    public StatusEnum Status { get; set; }
    public Guid CreatedUserId { get; set; }
    
    public ICollection<BalanceResponse> Balances { get; set; }
    [Required]
    [StringLength(32, MinimumLength = 32)]
    public string Version { get; set; } = VersionExtension.GenerateVersion();
}