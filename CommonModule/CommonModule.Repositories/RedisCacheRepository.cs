using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Repositories;

public class RedisCacheRepository<TId, TEntity> : ICacheRepository<TId, TEntity>
    where TId : notnull
    where TEntity : class, IBaseIdEntity<TId>
{
    private readonly ICacheBaseRepository<TId> cacheBaseRepository;
    private readonly string dictionaryName;

    public RedisCacheRepository(
        ICacheBaseRepository<TId> cacheBaseRepository
        )
    {
        this.cacheBaseRepository = cacheBaseRepository;
        this.dictionaryName = typeof(TEntity).Name.ToLower();
    }

    public async Task<List<TEntity>> GetItemsFromCacheAsync()
    {
        IEnumerable<string> items = await cacheBaseRepository.GetItemsFromCacheAsync(this.dictionaryName);
        
        return items?
            .Select(result => JsonSerializerExtension.FromString<TEntity>(result))
            .Where(entity => entity != null)
            .ToList() ?? new List<TEntity>();
    }

    private async Task<IEnumerable<RedisKey>> GetAllKeysAsync()
    {
        return await this.cacheBaseRepository.GetAllKeysAsync(this.dictionaryName);
    }

    public async Task ReinitializeDictionaryAsync(List<TEntity> values)
    {
        await this.cacheBaseRepository.ReinitializeDictionaryAsync(this.dictionaryName, values.ToDictionary(item => item.Id, item => JsonSerializerExtension.ToString(item)));
    }

    public async Task<string?> GetCacheVersionAsync()
    {
        return await this.cacheBaseRepository.GetCacheVersionAsync(this.dictionaryName);
    }

    public async Task SetCacheVersionAsync()
    {
        await this.cacheBaseRepository.SetCacheVersionAsync(this.dictionaryName);
    }
}