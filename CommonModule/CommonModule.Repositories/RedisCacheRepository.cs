using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Repositories;

public class RedisCacheRepository<TEntityId, TEntity> : ICacheRepository<TEntityId, TEntity>
    where TEntityId : notnull
    where TEntity : class, IBaseIdEntity<TEntityId>
{
    private readonly ICacheBaseRepository<TEntityId> cacheBaseRepository;
    private readonly string dictionaryName;

    public RedisCacheRepository(
        ICacheBaseRepository<TEntityId> cacheBaseRepository
        )
    {
        this.cacheBaseRepository = cacheBaseRepository;
        this.dictionaryName = typeof(TEntity).Name.Replace("Entity", "").ToLower();
    }

    public async Task<List<TEntity>?> ItemsFromCacheAsync()
    {
        IEnumerable<string> items = await cacheBaseRepository.ItemsFromCacheAsync(this.dictionaryName);

        return items
            .Select(result => JsonSerializerExtension.FromString<TEntity?>(result))
            .Where(entity => entity != null)
            .ToList();
    }

    private IEnumerable<RedisKey> AllKeys()
    {
        return this.cacheBaseRepository.AllKeys(this.dictionaryName);
    }

    public async Task ReinitializeDictionaryAsync(List<TEntity> values)
    {
        await this.cacheBaseRepository.ReinitializeDictionaryAsync(this.dictionaryName, values.ToDictionary(item => item.Id, item => JsonSerializerExtension.ToString(item)));
    }

    public async Task<string> CacheVersionAsync()
    {
        string? version = await this.cacheBaseRepository.CacheVersionAsync(this.dictionaryName);

        if (string.IsNullOrEmpty(version))
        {
            await this.cacheBaseRepository.SetCacheVersionAsync(this.dictionaryName);
        }
        
        version = await this.cacheBaseRepository.CacheVersionAsync(this.dictionaryName);

        if (string.IsNullOrEmpty(version))
        {
            throw new VersionException();
        }

        return version;
    }

    public async Task SetCacheVersionAsync()
    {
        await this.cacheBaseRepository.SetCacheVersionAsync(this.dictionaryName);
    }
}