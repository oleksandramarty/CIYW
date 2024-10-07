using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using CommonModule.Interfaces;
using CommonModule.Shared.Common.Auth;

namespace CommonModule.Repositories;

public class DynamoDbTokenRepository: ITokenRepository
{
    private readonly IDynamoDBContext context;
    private readonly IAmazonDynamoDB dynamoDbClient;
    private readonly IJwtTokenFactory jwtTokenFactory;

    public DynamoDbTokenRepository(
        IAmazonDynamoDB dynamoDbClient, 
        IDynamoDBContext context,
        IJwtTokenFactory jwtTokenFactory)
    {
        this.dynamoDbClient = dynamoDbClient;
        this.context = context;
        this.jwtTokenFactory = jwtTokenFactory;
    }

    public async Task AddTokenAsync(string token, TimeSpan expiration)
    {
        var userId = this.jwtTokenFactory.GetUserIdFromToken(token);
        var tokenItem = new TokenItemEntity()
        {
            UserId = userId,
            Token = token,
            Expiration = DateTime.UtcNow.Add(expiration)
        };

        await this.context.SaveAsync(tokenItem);
    }

    public async Task<bool> IsTokenValidAsync(string token)
    {
        var userId = this.jwtTokenFactory.GetUserIdFromToken(token);
        var tokenItem = await this.context.LoadAsync<TokenItemEntity>(userId, token);
        return tokenItem != null && tokenItem.Expiration > DateTime.UtcNow;
    }

    public async Task RemoveTokenAsync(string token)
    {
        var userId = this.jwtTokenFactory.GetUserIdFromToken(token);
        await this.context.DeleteAsync<TokenItemEntity>(userId, token);
    }

    public async Task RemoveUserTokenAsync(Guid userId)
    {
        var tokens = await this.context.QueryAsync<TokenItemEntity>(userId.ToString()).GetRemainingAsync();
        foreach (var token in tokens)
        {
            await this.context.DeleteAsync(token);
        }
    }

    public async Task RemoveAllTokensAsync(Guid userId)
    {
        await RemoveUserTokenAsync(userId);
    }

    public bool IsTokenExpired(string token)
    {
        var userId = this.jwtTokenFactory.GetUserIdFromToken(token);
        var tokenItem = this.context.LoadAsync<TokenItemEntity>(userId, token).Result;
        return tokenItem == null || tokenItem.Expiration <= DateTime.UtcNow;
    }
}