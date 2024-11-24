using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Core;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Enums.Expenses;

namespace CommonModule.Shared.Responses.Expenses.Models.Balances;

public class BalanceResponse: BaseDateTimeEntity<Guid>, IBaseVersionEntity, IStatusEntity
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public int CurrencyId { get; set; }
    public string? Title { get; set; }
    public int IconId { get; set; }
    
    public Guid UserProjectId { get; set; }
    [Required]
    [StringLength(32, MinimumLength = 32)]
    public string Version { get; set; } = VersionExtension.GenerateVersion();
    
    public int BalanceTypeId { get; set; }
    public StatusEnum Status { get; set; }
}