using System.Reflection;
using Autofac;
using Dictionaries.Mediatr.Mediatr.Categories.Categories.Requests;
using MediatR;

namespace Dictionaries.Mediatr;

public class MediatrDictionariesModule: Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();
        
        builder.RegisterAssemblyTypes(typeof(GetCategoriesRequest).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
    }
}