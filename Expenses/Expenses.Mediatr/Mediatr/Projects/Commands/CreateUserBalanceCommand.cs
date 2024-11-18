using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Enums.Expenses;
using CommonModule.Shared.Responses.Base;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Commands;

public class CreateUserBalanceCommand: IRequest<BaseEntityIdResponse<Guid>>
{
    public int CurrencyId { get; set; }
    [Required] [MaxLength(100)] public required string Title { get; set; }
    public int IconId { get; set; }
    public int BalanceTypeId { get; set; }
    public bool IsActive { get; set; }
    public Guid UserProjectId { get; set; }
}