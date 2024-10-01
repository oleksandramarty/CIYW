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

    public async Task<Dictionary<string, Dictionary<string, string>>> GetLocalizationDataAllAsync(bool isPublic)
    {
        var server = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().First());
        var keys = isPublic
            ? server.Keys(pattern: $"{this.instanceName}:*:*:{1}")
            : server.Keys(pattern: $"{this.instanceName}:*:*:{0}");

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

    public async Task<string> GetLocalizationVersionAsync(bool isPublic)
    {
        var redisKey = $"version:localization" + (isPublic ? "_public" : string.Empty);
        string version = await database.StringGetAsync(redisKey);

        return version;
    }
    
    public async Task SetLocalizationVersionAsync(bool isPublic)
    {
        string redisKey = $"version:localization" + (isPublic ? "_public" : string.Empty);
        await database.StringSetAsync(redisKey, Guid.NewGuid().ToString("N").ToUpper());
    }
}