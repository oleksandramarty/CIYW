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
        this.CreateMap<ExpenseEntity, ExpenseResponse>();
        this.CreateMap<PlannedExpenseEntity, PlannedExpenseResponse>();
        this.CreateMap<FavoriteExpenseEntity, FavoriteExpenseResponse>();
        this.CreateMap<CreateUserProjectCommand, UserProjectEntity>();
        this.CreateMap<UpdateUserProjectCommand, UserProjectEntity>();

        this.CreateMap<CreateUserBalanceCommand, BalanceEntity>();
        this.CreateMap<UpdateUserBalanceCommand, BalanceEntity>();

        this.CreateMap<CreateExpenseCommand, ExpenseEntity>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToUniversalTime()));
        this.CreateMap<UpdateExpenseCommand, ExpenseEntity>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToUniversalTime()));
        this.CreateMap<CreatePlannedExpenseCommand, PlannedExpenseEntity>()
            .ForMember(dest => dest.NextDate, opt => opt.MapFrom(src => src.StartDate.GetNextDate(src.FrequencyId)));
        this.CreateMap<UpdatePlannedExpenseCommand, PlannedExpenseEntity>()
            .ForMember(dest => dest.NextDate, opt => opt.MapFrom(src => src.StartDate.GetNextDate(src.FrequencyId)));
        this.CreateMap<CreateFavoriteExpenseCommand, FavoriteExpenseEntity>();
        this.CreateMap<UpdateFavoriteExpenseCommand, FavoriteExpenseEntity>();

        this.CreateMap<UserProjectEntity, UserProjectResponse>()
            .ForMember(dest => dest.Balances, opt => opt.MapFrom(src => src.Balances));

        this.CreateMap<BalanceEntity, BalanceResponse>();

        this.CreateMap<UserAllowedProjectEntity, UserAllowedProjectResponse>()
            .ForMember(dest => dest.UserProject, opt => opt.MapFrom(src => src.UserProject));
    }
}