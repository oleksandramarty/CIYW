using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Responses.Base;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Commands;

public class CreateUserProjectCommand: IRequest<BaseEntityIdResponse<Guid>>
{
    [Required] [MaxLength(100)] public required string Title { get; set; }
}