using System.Reflection;
using Autofac;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using MediatR;

namespace Expenses.Mediatr;

public class MediatrExpensesModule: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();
        
        builder.RegisterAssemblyTypes(typeof(CreateUserProjectCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<>));
        builder.RegisterAssemblyTypes(typeof(UpdateUserProjectCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<>));
        builder.RegisterAssemblyTypes(typeof(CreateUserBalanceCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<>));
        builder.RegisterAssemblyTypes(typeof(UpdateUserBalanceCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<>));
        builder.RegisterAssemblyTypes(typeof(RemoveUserBalanceCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        
        builder.RegisterAssemblyTypes(typeof(CreateExpenseCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<>));
        builder.RegisterAssemblyTypes(typeof(UpdateExpenseCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<>));
        builder.RegisterAssemblyTypes(typeof(GetFilteredExpensesRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(RemoveExpenseCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        
        builder.RegisterAssemblyTypes(typeof(CreatePlannedExpenseCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<>));
        builder.RegisterAssemblyTypes(typeof(UpdatePlannedExpenseCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<>));
        builder.RegisterAssemblyTypes(typeof(GetFilteredPlannedExpensesRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(RemovePlannedExpenseCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
               
        builder.RegisterAssemblyTypes(typeof(CreateFavoriteExpenseCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<>));
        builder.RegisterAssemblyTypes(typeof(UpdateFavoriteExpenseCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<>));
        builder.RegisterAssemblyTypes(typeof(GetFilteredFavoriteExpensesRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(RemoveFavoriteExpenseCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        
        builder.RegisterAssemblyTypes(typeof(GetUserProjectByIdRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(GetFilteredUserProjectsRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(GetFilteredUserAllowedProjectsRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
    }
}