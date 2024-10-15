using CommonModule.Shared.Common.BaseInterfaces;
using StackExchange.Redis;

namespace CommonModule.Interfaces;

public interface ICacheBaseRepository<TId> where TId : notnull
{
    Task<IEnumerable<string>> GetItemsFromCacheAsync(string dictionaryName);
    Task<IEnumerable<RedisKey>> GetAllKeysAsync(string dictionaryName);
    Task ReinitializeDictionaryAsync(string dictionaryName, Dictionary<TId, string> dictionary);
    Task<string?> GetCacheVersionAsync(string dictionaryName);
    Task SetCacheVersionAsync(string dictionaryName);
    Task<string> GetItemFromCacheAsync(string dictionaryName, TId key);
}