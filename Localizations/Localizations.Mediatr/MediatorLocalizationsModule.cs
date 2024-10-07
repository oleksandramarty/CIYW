using System.Reflection;
using Autofac;
using Localizations.Mediatr.Mediatr.Locations.Requests;
using MediatR;

namespace Localizations.Mediatr;

public class MediatorLocalizationsModule: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(typeof(GetLocalesRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(GetLocalizationsRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
    }
}