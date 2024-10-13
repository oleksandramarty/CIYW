using System.Reflection;
using AutoMapper;
using CommonModule.Shared.Common.BaseInterfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommonModule.Core.Extensions;

public static class MapperExtension
{
    public static TEntity CreateOrUpdateEntity<TCommand, TEntity, TId>(this Profile profile, TCommand src, ResolutionContext ctx)
        where TEntity : IBaseDateTimeEntity<TId>, IBaseVersionEntity, new()
    {
        TEntity entity = new TEntity();
        
        entity.Version = Guid.NewGuid().ToString("N").ToUpper();

        if (ctx.Items["IsUpdate"] is bool isUpdate && isUpdate)
        {
            entity.Modified = DateTime.UtcNow;
            return entity;
        }

        entity.Created = DateTime.UtcNow;
        
        // If TEntity has a "IsActive" property
        PropertyInfo isActiveProperty = typeof(TEntity).GetProperty("IsActive");
        if (isActiveProperty != null && isActiveProperty.PropertyType == typeof(bool))
        {
            isActiveProperty.SetValue(entity, true);
        }

        return entity;
    }
}