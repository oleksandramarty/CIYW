using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Core;

namespace CommonModule.Shared.Responses.Expenses.Models.Expenses;

public class ExpenseResponse: BaseDateTimeEntity<Guid>, IBaseVersionEntity
{
    [MaxLength(100)]
    public string? Title { get; set; }
    [MaxLength(300)]
    public string? Description { get; set; }
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
    public Guid BalanceId { get; set; }
    public DateTime Date { get; set; }
    public int CategoryId { get; set; }
    
    public Guid UserProjectId { get; set; }
    
    public Guid CreatedUserId { get; set; }
    [Required]
    [StringLength(32, MinimumLength = 32)]
    public string Version { get; set; } = VersionExtension.GenerateVersion();
    
    public Guid? FavoriteExpenseId { get; set; }
    
    public FavoriteExpenseResponse? FavoriteExpense { get; set; }
}