using AutoMapper;
using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Repositories;

public class DictionaryRepository<TEntityId, TEntity, TResponse, TDataContext>: IDictionaryRepository<TEntityId, TEntity, TResponse, TDataContext>
    where TEntityId : struct
    where TEntity : class, IBaseIdEntity<TEntityId>, IActivatableEntity
    where TResponse : class, IBaseIdEntity<TEntityId>
    where TDataContext : DbContext
{
    private readonly IMapper mapper;
    private readonly ICacheRepository<TEntityId, TEntity> cacheRepository;
    private readonly IReadGenericRepository<TEntityId, TEntity, TDataContext> dictionaryRepository;
    
    public DictionaryRepository(
        IMapper mapper,
        ICacheRepository<TEntityId, TEntity> cacheRepository,
        IReadGenericRepository<TEntityId, TEntity, TDataContext> dictionaryRepository
        )
    {
        this.mapper = mapper;
        this.cacheRepository = cacheRepository;
        this.dictionaryRepository = dictionaryRepository;
    }

    public async Task<VersionedListResponse<TResponse>> DictionaryAsync(
        string? version, 
        CancellationToken cancellationToken, 
        params Func<IQueryable<TEntity>, IQueryable<TEntity>>[]? includeFuncs)
    {
        string currentVersion = await this.cacheRepository.CacheVersionAsync();
        
        if (LocalizationExtension.IsDictionaryActual(version, currentVersion))
        {
            return new VersionedListResponse<TResponse>
            {
                Items = new List<TResponse>(),
                Version = currentVersion
            };
        }
        
        var items = await this.cacheRepository.ItemsFromCacheAsync();
    
        if (items == null || items.Count == 0)
        {
            items = await dictionaryRepository.ListAsync(null, cancellationToken, includeFuncs);
            await this.cacheRepository.ReinitializeDictionaryAsync(items);
            await this.cacheRepository.SetCacheVersionAsync();
        }
        
        if (string.IsNullOrEmpty(currentVersion))
        {
            currentVersion = await this.cacheRepository.CacheVersionAsync();
        }
    
        VersionedListResponse<TResponse> result = new VersionedListResponse<TResponse>
        {
            Items = items.Where(i => i.IsActive).Select(r => mapper.Map<TEntity, TResponse>(r)).ToList(),
            Version = currentVersion
        };

        return result;
    }
}