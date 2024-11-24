using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Core;
using CommonModule.Shared.Enums;
using Expenses.Domain.Models.Projects;

namespace Expenses.Domain.Models.Balances;

public class BalanceEntity : BaseDateTimeEntity<Guid>, IBaseVersionEntity, IStatusEntity
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public int CurrencyId { get; set; }
    [MaxLength(50)] public string? Title { get; set; }
    public int IconId { get; set; }

    public Guid UserProjectId { get; set; }
    public UserProjectEntity? UserProject { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 32)]
    public string Version { get; set; } = VersionExtension.GenerateVersion();

    public int BalanceTypeId { get; set; }
    public StatusEnum Status { get; set; }
}