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
        this.CreateMap<FavoriteExpense, FavoriteExpenseResponse>();
        this.CreateMap<CreateUserProjectCommand, UserProject>()
            .ConstructUsing((src, ctx) =>
                this.CreateOrUpdateEntity<CreateUserProjectCommand, UserProject, Guid>(src, ctx));
        this.CreateMap<UpdateUserProjectCommand, UserProject>()
            .ConstructUsing((src, ctx) =>
                this.CreateOrUpdateEntity<UpdateUserProjectCommand, UserProject, Guid>(src, ctx));

        this.CreateMap<CreateUserBalanceCommand, Balance>()
            .ConstructUsing((src, ctx) =>
                this.CreateOrUpdateEntity<CreateUserBalanceCommand, Balance, Guid>(src, ctx));
        this.CreateMap<UpdateUserBalanceCommand, Balance>()
            .ConstructUsing((src, ctx) =>
                this.CreateOrUpdateEntity<UpdateUserBalanceCommand, Balance, Guid>(src, ctx));

        this.CreateMap<CreateExpenseCommand, Expense>()
            .ConstructUsing((src, ctx) =>
                this.CreateOrUpdateEntity<CreateExpenseCommand, Expense, Guid>(src, ctx));
        this.CreateMap<CreatePlannedExpenseCommand, PlannedExpense>()
            .ConstructUsing((src, ctx) =>
                this.CreateOrUpdateEntity<CreatePlannedExpenseCommand, PlannedExpense, Guid>(src, ctx));
        this.CreateMap<CreateFavoriteExpenseCommand, FavoriteExpense>()
            .ConstructUsing((src, ctx) =>
                this.CreateOrUpdateEntity<CreateFavoriteExpenseCommand, FavoriteExpense, Guid>(src, ctx));
        this.CreateMap<UpdateExpenseCommand, Expense>()
            .ConstructUsing((src, ctx) =>
                this.CreateOrUpdateEntity<UpdateExpenseCommand, Expense, Guid>(src, ctx));
        this.CreateMap<UpdatePlannedExpenseCommand, PlannedExpense>()
            .ConstructUsing((src, ctx) =>
                this.CreateOrUpdateEntity<UpdatePlannedExpenseCommand, PlannedExpense, Guid>(src, ctx));        
        this.CreateMap<UpdateFavoriteExpenseCommand, FavoriteExpense>()
            .ConstructUsing((src, ctx) =>
                this.CreateOrUpdateEntity<UpdateFavoriteExpenseCommand, FavoriteExpense, Guid>(src, ctx));

        this.CreateMap<UserProject, UserProjectResponse>()
            .ForMember(dest => dest.Balances, opt => opt.MapFrom(src => src.Balances));

        this.CreateMap<Balance, BalanceResponse>();

        this.CreateMap<UserAllowedProject, UserAllowedProjectResponse>()
            .ForMember(dest => dest.UserProject, opt => opt.MapFrom(src => src.UserProject));
    }
}