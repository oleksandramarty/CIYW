using AutoMapper;
using CommonModule.Core.Extensions;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Categories.Commands;

namespace Expenses.Business;

public class MappingExpensesProfile: Profile
{
    public MappingExpensesProfile()
    {
        this.CreateMap<CreateUserProjectCommand, UserProject>()
            .ConstructUsing((src, ctx) => 
                this.CreateOrUpdateEntity<CreateUserProjectCommand, UserProject>(src, ctx));
    }
}