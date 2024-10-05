using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using Expenses.Domain.Models.Projects;

namespace Expenses.Domain.Models.Expenses;

public class PlannedExpense: BaseDateTimeEntity<Guid>, IActivatable, IBaseVersionEntity
{
    [MaxLength(50)]
    public string Title { get; set; }
    [MaxLength(100)]
    public string? Description { get; set; }
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
    public int CategoryId { get; set; }
    public Guid BalanceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime NextDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid UserProjectId { get; set; }
    public UserProject UserProject { get; set; }
    
    public int FrequencyId { get; set; }
    
    public bool IsActive { get; set; }
    public string Version { get; set; }
}