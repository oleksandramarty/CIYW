using AutoMapper;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Repositories;

public class DictionaryRepository<TId, TEntity, TResponse, TDataContext>: IDictionaryRepository<TId, TEntity, TResponse, TDataContext>
    where TEntity : class, IBaseIdEntity<TId>, IActivatable
    where TResponse : class, IBaseIdEntity<TId>
    where TDataContext : DbContext
{
    private readonly IMapper mapper;
    private readonly ICacheRepository<TId, TEntity> cacheRepository;
    private readonly IReadGenericRepository<TId, TEntity, TDataContext> dictionaryRepository;
    
    public DictionaryRepository(
        IMapper mapper,
        ICacheRepository<TId, TEntity> cacheRepository,
        IReadGenericRepository<TId, TEntity, TDataContext> dictionaryRepository
        )
    {
        this.mapper = mapper;
        this.cacheRepository = cacheRepository;
        this.dictionaryRepository = dictionaryRepository;
    }

    public async Task<VersionedList<TResponse>> GetDictionaryAsync(string? count, string? version, CancellationToken cancellationToken)
    {
        BaseVersionEntity baseVersionEntity = await this.cacheRepository.GetCacheVersionAsync();
        
        if (
            !string.IsNullOrEmpty(version) && 
            version.Equals(baseVersionEntity.Version) && 
            !string.IsNullOrEmpty(count) && 
            count.Equals(baseVersionEntity.Count))
        {
            return new VersionedList<TResponse>
            {
                Items = new List<TResponse>(),
                Version = baseVersionEntity.Version,
                Count = baseVersionEntity.Count
            };
        }
        
        var items = await this.cacheRepository.GetItemsFromCacheAsync();
    
        if (items == null || items.Count == 0)
        {
            items = await dictionaryRepository.GetListAsync(null, cancellationToken);
            await this.cacheRepository.ReinitializeDictionaryAsync(items);
            await this.cacheRepository.SetCacheVersionAsync(new BaseVersionEntity
            {
                Count = items.Count.ToString(),
                Version = DateTime.UtcNow.ToString("yyyyMMddHHmmss")
            });
        }
    
        return new VersionedList<TResponse>
        {
            Items = items.Where(i => i.IsActive).Select(r => mapper.Map<TEntity, TResponse>(r)).ToList(),
            Version = baseVersionEntity.Version,
            Count = baseVersionEntity.Count
        };
    }
}