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

namespace Expenses.Mediatr;

public class MappingExpensesProfile : Profile
{
    public MappingExpensesProfile()
    {
        this.CreateMap<Expense, ExpenseResponse>();
        this.CreateMap<PlannedExpense, PlannedExpenseResponse>();
        this.CreateMap<FavoriteExpense, FavoriteExpenseResponse>();
        this.CreateMap<CreateUserProjectCommand, UserProject>();
        this.CreateMap<UpdateUserProjectCommand, UserProject>();

        this.CreateMap<CreateUserBalanceCommand, Balance>();
        this.CreateMap<UpdateUserBalanceCommand, Balance>();

        this.CreateMap<CreateExpenseCommand, Expense>();
        this.CreateMap<UpdateExpenseCommand, Expense>();
        this.CreateMap<CreatePlannedExpenseCommand, PlannedExpense>()
            .ForMember(dest => dest.NextDate, opt => opt.MapFrom(src => src.StartDate.GetNextDate(src.FrequencyId)));
        this.CreateMap<UpdatePlannedExpenseCommand, PlannedExpense>()
            .ForMember(dest => dest.NextDate, opt => opt.MapFrom(src => src.StartDate.GetNextDate(src.FrequencyId)));
        this.CreateMap<CreateFavoriteExpenseCommand, FavoriteExpense>();
        this.CreateMap<UpdateFavoriteExpenseCommand, FavoriteExpense>();

        this.CreateMap<UserProject, UserProjectResponse>()
            .ForMember(dest => dest.Balances, opt => opt.MapFrom(src => src.Balances));

        this.CreateMap<Balance, BalanceResponse>();

        this.CreateMap<UserAllowedProject, UserAllowedProjectResponse>()
            .ForMember(dest => dest.UserProject, opt => opt.MapFrom(src => src.UserProject));
    }
}