using System.ComponentModel.DataAnnotations;
using CommonModule.Shared.Common;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Commands;

public class UpdateUserProjectCommand : BaseIdEntity<Guid>, IRequest
{
    [Required] [MaxLength(100)] public required string Title { get; set; }
}