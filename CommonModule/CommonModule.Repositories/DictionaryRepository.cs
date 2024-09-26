using AutoMapper;
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

    public async Task<VersionedList<TResponse>> GetDictionaryAsync(CancellationToken cancellationToken)
    {
        var items = await this.cacheRepository.GetItemsFromCacheAsync();
    
        if (items == null || items.Count == 0)
        {
            items = await dictionaryRepository.GetListAsync(null, cancellationToken);
            await this.cacheRepository.ReinitializeDictionaryAsync(items);
            await this.cacheRepository.SetCacheVersionAsync();
        }
    
        var currentVersion = await this.cacheRepository.GetCacheVersionAsync();
    
        return new VersionedList<TResponse>
        {
            Items = items.Where(i => i.IsActive).Select(r => mapper.Map<TEntity, TResponse>(r)).ToList(),
            Version = currentVersion
        };
    }
}