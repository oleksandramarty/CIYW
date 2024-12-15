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
        CreateMap<ExpenseEntity, ExpenseResponse>();
        CreateMap<PlannedExpenseEntity, PlannedExpenseResponse>();
        CreateMap<FavoriteExpenseEntity, FavoriteExpenseResponse>();
        CreateMap<CreateUserProjectCommand, UserProjectEntity>();
        CreateMap<UpdateUserProjectCommand, UserProjectEntity>();

        CreateMap<CreateUserBalanceCommand, BalanceEntity>();
        CreateMap<UpdateUserBalanceCommand, BalanceEntity>();

        CreateMap<CreateExpenseCommand, ExpenseEntity>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToUniversalTime()));
        CreateMap<UpdateExpenseCommand, ExpenseEntity>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToUniversalTime()));
        CreateMap<CreatePlannedExpenseCommand, PlannedExpenseEntity>()
            .ForMember(dest => dest.NextDate, opt => opt.MapFrom(src => src.StartDate.GetNextDate(src.FrequencyId)));
        CreateMap<UpdatePlannedExpenseCommand, PlannedExpenseEntity>()
            .ForMember(dest => dest.NextDate, opt => opt.MapFrom(src => src.StartDate.GetNextDate(src.FrequencyId)));
        CreateMap<CreateFavoriteExpenseCommand, FavoriteExpenseEntity>();
        CreateMap<UpdateFavoriteExpenseCommand, FavoriteExpenseEntity>();

        CreateMap<UserProjectEntity, UserProjectResponse>()
            .ForMember(dest => dest.Balances, opt => opt.MapFrom(src => src.Balances));

        CreateMap<BalanceEntity, BalanceResponse>();

        CreateMap<UserAllowedProjectEntity, UserAllowedProjectResponse>()
            .ForMember(dest => dest.UserProject, opt => opt.MapFrom(src => src.UserProject));
    }
}