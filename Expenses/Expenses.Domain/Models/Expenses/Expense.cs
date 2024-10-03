using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using Expenses.Domain.Models.Projects;

namespace Expenses.Domain.Models.Expenses;

public class Expense: BaseDateTimeEntity<Guid>, IBaseVersionEntity
{
    [MaxLength(100)]
    public string Title { get; set; }
    [MaxLength(300)]
    public string? Description { get; set; }
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
    public Guid BalanceId { get; set; }
    public DateTime Date { get; set; }
    public int CategoryId { get; set; }
    public Guid UserProjectId { get; set; }
    public UserProject UserProject { get; set; }
    
    public Guid CreatedUserId { get; set; }
    public string Version { get; set; }
}