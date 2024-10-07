using CommonModule.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace CommonModule.Repositories;

public class RedisTokenRepository : ITokenRepository
{
    private readonly IDatabase database;
    private readonly IJwtTokenFactory jwtTokenFactory;
    private readonly string instanceName;

    public RedisTokenRepository(
        IConnectionMultiplexer connectionMultiplexer,
        IConfiguration configuration,
        IJwtTokenFactory jwtTokenFactory)
    {
        this.database = connectionMultiplexer.GetDatabase();
        this.jwtTokenFactory = jwtTokenFactory;

        this.instanceName = configuration["Redis:InstanceNameToken"];
    }

    public async Task AddTokenAsync(string token, TimeSpan expiration)
    {
        string key = $"{this.instanceName}:{this.jwtTokenFactory.GetUserIdFromToken(token)}:{token}";
        await database.StringSetAsync(key, "valid", expiration);
    }

    public async Task<bool> IsTokenValidAsync(string token)
    {
        string key = $"{this.instanceName}:{this.jwtTokenFactory.GetUserIdFromToken(token)}:{token}";
        return await database.KeyExistsAsync(key);
    }

    public async Task RemoveTokenAsync(string token)
    {
        string key = $"{this.instanceName}:{this.jwtTokenFactory.GetUserIdFromToken(token)}:{token}";
        await database.KeyDeleteAsync(key);
    }

    public async Task RemoveUserTokenAsync(Guid userId)
    {
        string pattern = $"{this.instanceName}:{userId.ToString()}:*";
        var keys = database.Multiplexer.GetServer(database.Multiplexer.GetEndPoints()[0]).Keys(pattern: pattern);
        foreach (var key in keys)
        {
            await database.KeyDeleteAsync(key);
        }
    }

    public async Task RemoveAllTokensAsync(Guid userId)
    {
        string key = $"{this.instanceName}:{userId.ToString()}:*";
        var keys = database.Multiplexer.GetServer(database.Multiplexer.GetEndPoints()[0]).Keys(pattern: key);
        foreach (var redisKey in keys)
        {
            await database.KeyDeleteAsync(redisKey);
        }
    }

    public bool IsTokenExpired(string token)
    {
        string key = $"{this.instanceName}:{this.jwtTokenFactory.GetUserIdFromToken(token)}:{token}";
        return !database.KeyExists(key);
    }
}