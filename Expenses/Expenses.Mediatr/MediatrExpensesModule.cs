using System.Reflection;
using Autofac;
using Expenses.Mediatr.Mediatr.Categories.Commands;
using MediatR;

namespace Expenses.Mediatr;

public class MediatrExpensesModule: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();
        
        builder.RegisterAssemblyTypes(typeof(CreateUserProjectCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
    }
}