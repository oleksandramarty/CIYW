using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Interfaces;

public interface ICacheRepository<TId, TEntity>
    where TId : notnull
    where TEntity : class, IBaseIdEntity<TId>
{
    Task<List<TEntity>> GetItemsFromCacheAsync();
    Task ReinitializeDictionaryAsync(List<TEntity> values);
    Task<string?> GetCacheVersionAsync();
    Task SetCacheVersionAsync();
}