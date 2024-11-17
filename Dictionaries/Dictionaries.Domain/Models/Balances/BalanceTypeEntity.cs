using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums.Expenses;

namespace Dictionaries.Domain.Models.Balances;

public class BalanceTypeEntity : BaseIdEntity<int>, IActivatableEntity
{
    [Required] [MaxLength(50)] public required string Title { get; set; }
    public bool IsActive { get; set; }
    public BalanceEnum Type { get; set; }
}