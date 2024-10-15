using System.Text.RegularExpressions;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Common.BaseInterfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CommonModule.Repositories;

public class RedisCacheBaseRepository<TId>: ICacheBaseRepository<TId> 
    where TId : notnull
{
    private readonly IConnectionMultiplexer connectionMultiplexer;
    private readonly IDatabase database;
    private readonly string instanceName;

    public RedisCacheBaseRepository(
        IConnectionMultiplexer connectionMultiplexer,
        IConfiguration configuration
        )
    {
        this.connectionMultiplexer = connectionMultiplexer;
        this.database = connectionMultiplexer.GetDatabase();
        this.instanceName = configuration["Redis:InstanceNameDictionary"];
    }

    public async Task<IEnumerable<string>> GetItemsFromCacheAsync(string dictionaryName)
    {
        var keys = await GetAllKeysAsync(dictionaryName);
        var tasks = keys.Select(key => database.StringGetAsync(key)).ToList();
        var results = await Task.WhenAll(tasks);
        
        return results
            .Where(result => !result.IsNullOrEmpty)
            .Select(r => r.ToString());
    }

    public async Task<IEnumerable<RedisKey>> GetAllKeysAsync(string dictionaryName)
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

    public async Task ReinitializeDictionaryAsync(string dictionaryName, Dictionary<TId, string> dictionary)
    {
        await database.KeyDeleteAsync($"{instanceName}:{dictionaryName}:*");

        var tasks = dictionary.Select(item =>
        {
            var redisKey = $"{instanceName}:{dictionaryName}:{item.Key}";
            return database.StringSetAsync(redisKey, item.Value);
        });

        await Task.WhenAll(tasks);
    }

    public async Task<string?> GetCacheVersionAsync(string dictionaryName)
    {
        var redisKey = $"version:{dictionaryName.ToLower()}";
        string? version = await database.StringGetAsync(redisKey);

        return version;
    }

    public async Task SetCacheVersionAsync(string dictionaryName)
    {
        var redisKey = $"version:{dictionaryName.ToLower()}";
        await database.StringSetAsync(redisKey, Guid.NewGuid().ToString("N").ToUpper());
    }

    public async Task<string> GetItemFromCacheAsync(string dictionaryName, TId key)
    {
        var redisKey = $"{instanceName}:{dictionaryName}:{key}";
        return await database.StringGetAsync(redisKey);
    }
}