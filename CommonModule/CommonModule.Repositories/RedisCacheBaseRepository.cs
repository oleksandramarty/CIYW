using CommonModule.Interfaces;
using CommonModule.Shared.Common;
using CommonModule.Shared.Core;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CommonModule.Repositories;

public class RedisCacheBaseRepository<TEntity>: AuditableNonNullableKey, ICacheBaseRepository<TEntity> 
    where TEntity : notnull
{
    private readonly IConnectionMultiplexer connectionMultiplexer;
    private readonly IDatabase database;

    public RedisCacheBaseRepository(
        IConnectionMultiplexer connectionMultiplexer,
        IConfiguration configuration
        )
    {
        this.connectionMultiplexer = connectionMultiplexer;
        this.database = connectionMultiplexer.GetDatabase();
        this.Key = configuration["Redis:InstanceNameDictionary"];
    }

    public async Task<IEnumerable<string>> ItemsFromCacheAsync(string dictionaryName)
    {
        var keys = AllKeys(dictionaryName);
        var tasks = keys.Select(key => database.StringGetAsync(key)).ToList();
        var results = await Task.WhenAll(tasks);
        
        return results
            .Where(result => !result.IsNullOrEmpty)
            .Select(r => r.ToString());
    }

    public IEnumerable<RedisKey> AllKeys(string dictionaryName)
    {
        var endpoints = connectionMultiplexer.GetEndPoints();
        var keys = new List<RedisKey>();

        foreach (var endpoint in endpoints)
        {
            var server = connectionMultiplexer.GetServer(endpoint);
            keys.AddRange(server.Keys(database.Database, $"{this.Key}:{dictionaryName}:*"));
        }

        return keys;
    }

    public async Task ReinitializeDictionaryAsync(string dictionaryName, Dictionary<TEntity, string> dictionary)
    {
        await database.KeyDeleteAsync($"{this.Key}:{dictionaryName}:*");

        var tasks = dictionary.Select(item =>
        {
            var redisKey = $"{this.Key}:{dictionaryName}:{item.Key}";
            return database.StringSetAsync(redisKey, item.Value);
        });

        await Task.WhenAll(tasks);
    }

    public async Task<string?> CacheVersionAsync(string dictionaryName)
    {
        var redisKey = $"version:{dictionaryName.ToLower()}";
        string? version = await database.StringGetAsync(redisKey);

        return version;
    }

    public async Task SetCacheVersionAsync(string dictionaryName)
    {
        var redisKey = $"version:{dictionaryName.ToLower()}";
        await database.StringSetAsync(redisKey, VersionExtension.GenerateVersion());
    }

    public async Task<string?> ItemFromCacheAsync(string dictionaryName, TEntity key)
    {
        var redisKey = $"{this.Key}:{dictionaryName}:{key}";
        return await database.StringGetAsync(redisKey);
    }
}