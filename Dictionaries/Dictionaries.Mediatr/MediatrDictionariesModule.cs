using System.Reflection;
using Autofac;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;

namespace Dictionaries.Mediatr;

public class MediatrDictionariesModule: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();
        
        builder.RegisterAssemblyTypes(typeof(GetCategoriesRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(GetCurrenciesRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(GetCountriesRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAssemblyTypes(typeof(GetFrequenciesRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
    }
}