using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using CommonModule.Shared.Responses.Categories;

namespace CommonModule.Shared.Responses.Expenses;

public class ExpenseResponse: BaseDateTimeEntity<Guid>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    
    public Guid UserCategoryId { get; set; }
    public UserCategoryResponse UserCategory { get; set; }
}