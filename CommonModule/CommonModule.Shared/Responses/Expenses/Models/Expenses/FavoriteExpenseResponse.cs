using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Core;

namespace CommonModule.Shared.Responses.Expenses.Models.Expenses;

public class FavoriteExpenseResponse: BaseDateTimeEntity<Guid>, IBaseVersionEntity
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? Limit { get; set; }
    public decimal? CurrentAmount { get; set; }
    public int? CategoryId { get; set; }
    public int? FrequencyId { get; set; }
    public int CurrencyId { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid UserProjectId { get; set; }
    public int IconId { get; set; }
    
    public Guid CreatedUserId { get; set; }
    [Required]
    [StringLength(32, MinimumLength = 32)]
    public string Version { get; set; } = VersionExtension.GenerateVersion();
    
    public ICollection<ExpenseResponse> Expenses { get; set; }
}