using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Expenses.Mediatr.Mediatr.Projects.Commands;

public class CreateUserProjectCommand: IRequest
{
    [Required] [MaxLength(100)] public required string Title { get; set; }
    public bool IsActive { get; set; }
}