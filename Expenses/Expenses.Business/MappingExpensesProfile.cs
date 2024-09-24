using AutoMapper;
using CommonModule.Core.Extensions;
using CommonModule.Shared.Responses.Expenses.Models.Balances;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;

namespace Expenses.Business;

public class MappingExpensesProfile: Profile
{
    public MappingExpensesProfile()
    {
        this.CreateMap<CreateUserProjectCommand, UserProject>()
            .ConstructUsing((src, ctx) => 
                this.CreateOrUpdateEntity<CreateUserProjectCommand, UserProject>(src, ctx));

        this.CreateMap<UserProject, UserProjectResponse>()
            .ForMember(dest => dest.Balances, 
                opt => opt.MapFrom(src => src.Balances != null 
                    ? src.Balances.Select(b => new BalanceResponse
                    {
                        Id = b.Id,
                        Amount = b.Amount,
                        Created = b.Created,
                        Modified = b.Modified,
                        UserProjectId = b.UserProjectId,
                        CurrencyId = b.CurrencyId,
                        UserId = b.UserId
                    }).ToList() : null));

        this.CreateMap<UserAllowedProject, UserAllowedProjectResponse>()
            .ForMember(dest => dest.UserProject, 
                opt => opt.MapFrom(src => src.UserProject != null 
                    ? new UserProjectResponse
                    {
                        Id = src.UserProject.Id,
                        Title = src.UserProject.Title,
                        IsActive = src.UserProject.IsActive,
                        Balances = src.UserProject.Balances.Select(b => 
                            new BalanceResponse
                            {
                                Id = b.Id,
                                Amount = b.Amount,
                                Created = b.Created,
                                Modified = b.Modified,
                                UserProjectId = b.UserProjectId,
                                CurrencyId = b.CurrencyId,
                                UserId = b.UserId
                            }).ToList(),
                    } 
                    : null));
    }
}