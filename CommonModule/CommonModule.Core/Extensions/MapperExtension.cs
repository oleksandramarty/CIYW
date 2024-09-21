using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommonModule.Core.Extensions;

public static class MapperExtension
{
    public static TEntity CreateOrUpdateEntity<TCommand, TEntity>(this Profile profile, TCommand src, ResolutionContext ctx)
        where TEntity : new()
    {
        TEntity entity = new TEntity();

        if (ctx.Items["IsUpdate"] is bool isUpdate && isUpdate)
        {
            return entity;
        }

        // If TEntity has an "Id" property
        PropertyInfo idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty != null && idProperty.PropertyType == typeof(Guid))
        {
            idProperty.SetValue(entity, Guid.NewGuid());
        }

        // If TEntity has a "Created" property
        PropertyInfo createdProperty = typeof(TEntity).GetProperty("Created");
        if (createdProperty != null && createdProperty.PropertyType == typeof(DateTime))
        {
            createdProperty.SetValue(entity, DateTime.UtcNow);
        }

        // If TEntity has a "IsActive" property
        PropertyInfo isActiveProperty = typeof(TEntity).GetProperty("IsActive");
        if (isActiveProperty != null && isActiveProperty.PropertyType == typeof(bool))
        {
            isActiveProperty.SetValue(entity, true);
        }

        return entity;
    }
}