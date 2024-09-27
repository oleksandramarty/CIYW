using System.Text.RegularExpressions;
using CommonModule.Interfaces;
using CommonModule.Shared.Common.BaseInterfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CommonModule.Repositories;

public class RedisLocalizationRepository : ILocalizationRepository
{
    private readonly IDatabase database;
    private readonly IConnectionMultiplexer connectionMultiplexer;
    private readonly string instanceName;

    private readonly string fallbackLocale = "en";
    
    public RedisLocalizationRepository(
        IConnectionMultiplexer connectionMultiplexer,
        IConfiguration configuration)
    {
        this.connectionMultiplexer = connectionMultiplexer;
        this.database = connectionMultiplexer.GetDatabase();

        this.instanceName = configuration["Redis:InstanceNameLocalization"];
    }

    public async Task SetLocalizationDataByLocaleAsync(string locale, string key, string value, bool isPublic = false)
    {
        string redisKey = $"{this.instanceName}:{locale}:{key}:{(isPublic ? 1 : 0)}";
        await database.StringSetAsync(redisKey, value);
    }

    public async Task SetLocalizationDataAllAsync(string locale, Dictionary<string, Tuple<string, bool>> data)
    {
        foreach (var kvp in data)
        {
            string key = $"{this.instanceName}:{locale}:{kvp.Key}:{(kvp.Value.Item2 ? 1 : 0)}";
            await database.StringSetAsync(key, kvp.Value.Item1);
        }
    }

    public async Task SetLocalizationDataAllAsync(Dictionary<string, Dictionary<string, Tuple<string, bool>>> data)
    {
        foreach (var localeData in data)
        {
            foreach (var kvp in localeData.Value)
            {
                string key = $"{this.instanceName}:{localeData.Key}:{kvp.Key}:{(kvp.Value.Item2 ? 1 : 0)}";
                await database.StringSetAsync(key, kvp.Value.Item1);
            }
        }
    }

    public async Task<string> GetLocalizationDataByKeyAsync(string locale, string key, bool isPublic = false)
    {
        string redisKey = $"{this.instanceName}:{locale}:{key}:{(isPublic ? 1 : 0)}";
        string value = await database.StringGetAsync(redisKey);

        if (string.IsNullOrEmpty(value))
        {
            string fallbackKey = $"{this.instanceName}:{fallbackLocale}:{key}:{(isPublic ? 1 : 0)}";
            value = await database.StringGetAsync(fallbackKey);
        }

        return value.ToString();
    }

    public async Task<Dictionary<string, string>> GetLocalizationDataByLocaleAsync(string locale, bool isPublic = false)
    {
        var server = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().First());
        var keys = isPublic
            ? server.Keys(pattern: $"{this.instanceName}:{locale}:*:{1}")
            : server.Keys(pattern: $"{this.instanceName}:{locale}:*:{0}")
                .Concat(server.Keys(pattern: $"{this.instanceName}:{fallbackLocale}:*:{1}"));

        var data = new Dictionary<string, string>();
        foreach (var key in keys)
        {
            var value = await database.StringGetAsync(key);
            data[key.ToString().Split(':')[2]] = value;
        }

        // Check for fallback locale if no data found
        if (data.Count == 0 && locale != fallbackLocale)
        {
            keys = isPublic
                ? server.Keys(pattern: $"{this.instanceName}:{fallbackLocale}:*:{1}")
                : server.Keys(pattern: $"{this.instanceName}:{fallbackLocale}:*:{0}")
                    .Concat(server.Keys(pattern: $"{this.instanceName}:{fallbackLocale}:*:{1}"));

            foreach (var key in keys)
            {
                var value = await database.StringGetAsync(key);
                data[key.ToString().Split(':')[2]] = value.ToString();
            }
        }

        return data;
    }

    public async Task<Dictionary<string, Dictionary<string, string>>> GetLocalizationDataAllAsync(bool isPublic = false)
    {
        var server = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().First());
        var keys = isPublic
            ? server.Keys(pattern: $"{this.instanceName}:*:*:{1}")
            : server.Keys(pattern: $"{this.instanceName}:*:*:{0}")
                .Concat(server.Keys(pattern: $"{this.instanceName}:*:*:{1}"));

        var data = new Dictionary<string, Dictionary<string, string>>();
        foreach (var key in keys)
        {
            var parts = key.ToString().Split(':');
            string locale = parts[1];
            string subKey = parts[2];
            var value = await database.StringGetAsync(key);

            if (!data.ContainsKey(locale))
            {
                data[locale] = new Dictionary<string, string>();
            }

            data[locale][subKey] = value.ToString();
        }

        return data;
    }

    public async Task<BaseVersionEntity> GetLocalizationVersionAsync()
    {
        var redisKey = $"{instanceName}:version:localization";
        string version = await database.StringGetAsync(redisKey);
        redisKey = $"{instanceName}:count:localization";
        string count = await database.StringGetAsync(redisKey);

        return new BaseVersionEntity
        {
            Count = count?.Replace(" ", ""),
            Version = version
        };
    }
    
    public async Task SetLocalizationVersionAsync()
    {
        string redisKey = $"{instanceName}:version:localization";
        await database.StringSetAsync(redisKey, $"{DateTime.UtcNow:yyyyMMddHHmmss}");
    }

    public async Task DeleteLocalizationDataByKeyAsync(string locale, string key)
    {
        string redisKeyPublic = $"{this.instanceName}:{locale}:{key}:1";
        string redisKeyPrivate = $"{this.instanceName}:{locale}:{key}:0";
        await database.KeyDeleteAsync(redisKeyPublic);
        await database.KeyDeleteAsync(redisKeyPrivate);
    }

    public async Task DeleteLocalizationDataByLocaleAsync(string locale)
    {
        var server = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().First());
        var keysPublic = server.Keys(pattern: $"{this.instanceName}:{locale}:*:1");
        var keysPrivate = server.Keys(pattern: $"{this.instanceName}:{locale}:*:0");

        foreach (var key in keysPublic)
        {
            await database.KeyDeleteAsync(key);
        }

        foreach (var key in keysPrivate)
        {
            await database.KeyDeleteAsync(key);
        }
    }

    public async Task DeleteLocalizationDataAllAsync()
    {
        var server = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().First());
        var keysPublic = server.Keys(pattern: $"{this.instanceName}:*:*:1");
        var keysPrivate = server.Keys(pattern: $"{this.instanceName}:*:*:0");

        foreach (var key in keysPublic)
        {
            await database.KeyDeleteAsync(key);
        }

        foreach (var key in keysPrivate)
        {
            await database.KeyDeleteAsync(key);
        }
    }
}