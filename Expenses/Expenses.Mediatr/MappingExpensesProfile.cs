using AutoMapper;
using CommonModule.Core.Extensions;
using CommonModule.Shared.Responses.Expenses.Models.Balances;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Mediatr.Projects.Commands;

namespace Expenses.Business;

public class MappingExpensesProfile : Profile
{
    public MappingExpensesProfile()
    {
        this.CreateMap<Expense, ExpenseResponse>();
        this.CreateMap<PlannedExpense, PlannedExpenseResponse>();
        this.CreateMap<CreateUserProjectCommand, UserProject>()
            .ConstructUsing((src, ctx) => 
                this.CreateOrUpdateEntity<CreateUserProjectCommand, UserProject, Guid>(src, ctx));
        
        this.CreateMap<CreateOrUpdateExpenseCommand, Expense>()
            .ConstructUsing((src, ctx) => 
                this.CreateOrUpdateEntity<CreateOrUpdateExpenseCommand, Expense, Guid>(src, ctx));
        this.CreateMap<CreateOrUpdatePlannedExpenseCommand, PlannedExpense>()
            .ConstructUsing((src, ctx) => 
                this.CreateOrUpdateEntity<CreateOrUpdatePlannedExpenseCommand, PlannedExpense, Guid>(src, ctx));

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
                    }).ToList() : new List<BalanceResponse>()));

        this.CreateMap<Balance, BalanceResponse>();

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