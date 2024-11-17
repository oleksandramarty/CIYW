using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Core;
using Expenses.Domain.Models.Projects;

namespace Expenses.Domain.Models.Expenses;

public class FavoriteExpenseEntity: BaseDateTimeEntity<Guid>, IBaseVersionEntity
{
    [MaxLength(50)] public string? Title { get; set; }
    [MaxLength(100)] public string? Description { get; set; }
    [Range(0.01, double.MaxValue)] public decimal? Limit { get; set; }
    public decimal? CurrentAmount { get; set; }
    public int? CategoryId { get; set; }
    public int? FrequencyId { get; set; }
    public int CurrencyId { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid UserProjectId { get; set; }
    public int IconId { get; set; }
    public UserProjectEntity? UserProject { get; set; }
    
    public Guid CreatedUserId { get; set; }
    [Required]
    [StringLength(32, MinimumLength = 32)]
    public string Version { get; set; } = VersionExtension.GenerateVersion();
    
    public ICollection<ExpenseEntity> Expenses { get; set; }
}