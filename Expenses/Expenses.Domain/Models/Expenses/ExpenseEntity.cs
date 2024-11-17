using System.ComponentModel.DataAnnotations;
using CommonModule.Core.Extensions;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Core;
using Expenses.Domain.Models.Projects;

namespace Expenses.Domain.Models.Expenses;

public class ExpenseEntity: BaseDateTimeEntity<Guid>, IBaseVersionEntity
{
    [MaxLength(50)] public string? Title { get; set; }
    [MaxLength(100)] public string? Description { get; set; }
    [Range(0.01, double.MaxValue)] public decimal Amount { get; set; }
    public Guid BalanceId { get; set; }
    public DateTime Date { get; set; }
    public int CategoryId { get; set; }
    public Guid UserProjectId { get; set; }
    public UserProjectEntity? UserProject { get; set; }
    
    public Guid CreatedUserId { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 32)]
    public string Version { get; set; } = VersionExtension.GenerateVersion();
    
    public Guid? FavoriteExpenseId { get; set; }
    
    public FavoriteExpenseEntity? FavoriteExpense { get; set; }
}