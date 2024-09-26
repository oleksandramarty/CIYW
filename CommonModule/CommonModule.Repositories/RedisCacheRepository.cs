using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Repositories;

public class RedisCacheRepository<TId, TEntity> : ICacheRepository<TId, TEntity>
    where TEntity : class, IBaseIdEntity<TId>
{
    private readonly IConnectionMultiplexer connectionMultiplexer;
    private readonly IDatabase database;
    private readonly string instanceName;
    private readonly string dictionaryName;

    public RedisCacheRepository(
        IConnectionMultiplexer connectionMultiplexer,
        IConfiguration configuration
        )
    {
        this.connectionMultiplexer = connectionMultiplexer;
        this.database = connectionMultiplexer.GetDatabase();
        this.instanceName = configuration["Redis:InstanceNameDictionary"];
        this.dictionaryName = typeof(TEntity).Name.ToLower();
    }

    public async Task<List<TEntity>> GetItemsFromCacheAsync()
    {
        var keys = await GetAllKeysAsync();
        var tasks = keys.Select(key => database.StringGetAsync(key)).ToList();
        var results = await Task.WhenAll(tasks);
        
        return results
            .Where(result => !result.IsNullOrEmpty)
            .Select(result => CacheExtension.FromCacheString<TEntity>(result))
            .Where(entity => entity != null)
            .ToList();
    }

    private async Task<IEnumerable<RedisKey>> GetAllKeysAsync()
    {
        var endpoints = connectionMultiplexer.GetEndPoints();
        var keys = new List<RedisKey>();

        foreach (var endpoint in endpoints)
        {
            var server = connectionMultiplexer.GetServer(endpoint);
            keys.AddRange(server.Keys(database.Database, $"{instanceName}:{dictionaryName}:*"));
        }

        return keys;
    }

    public async Task ReinitializeDictionaryAsync(List<TEntity> values)
    {
        await database.KeyDeleteAsync($"{instanceName}:{dictionaryName}:*");

        var tasks = values.Select(value =>
        {
            var redisKey = $"{instanceName}:{dictionaryName}:{value.Id}";
            return database.StringSetAsync(redisKey, CacheExtension.ToCacheString(value));
        });

        await Task.WhenAll(tasks);
    }

    public async Task<string> GetCacheVersionAsync()
    {
        var redisKey = $"{instanceName}:version:{dictionaryName}";
        return await database.StringGetAsync(redisKey);
    }

    public async Task SetCacheVersionAsync()
    {
        var redisKey = $"{instanceName}:version:{dictionaryName}";
        await database.StringSetAsync(redisKey, $"{DateTime.UtcNow:yyyyMMddHHmmss}");
    }
}