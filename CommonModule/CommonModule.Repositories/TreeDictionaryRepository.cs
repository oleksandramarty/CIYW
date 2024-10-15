using AutoMapper;
using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Repositories;

public class TreeDictionaryRepository<TId, TParentId, TEntity, TResponse, TDataContext>: ITreeDictionaryRepository<TId, TParentId, TEntity, TResponse, TDataContext>
    where TEntity : class, ITreeEntity<TId, TParentId>, IActivatable
    where TResponse : class, ITreeChildren<TResponse>
    where TDataContext : DbContext
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<TDataContext> entityValidator;
    private readonly ICacheRepository<TId, TEntity> cacheRepository;
    private readonly IReadGenericRepository<TId, TEntity, TDataContext> dictionaryRepository;
    
    public TreeDictionaryRepository(
        IMapper mapper,
        IEntityValidator<TDataContext> entityValidator,
        ICacheRepository<TId, TEntity> cacheRepository,
        IReadGenericRepository<TId, TEntity, TDataContext> dictionaryRepository,
        IKafkaMessageService kafkaMessageService
        )
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.cacheRepository = cacheRepository;
        this.dictionaryRepository = dictionaryRepository;
    }
    
    public async Task<VersionedListResponse<TResponse>> GetTreeDictionaryAsync(string? version, CancellationToken cancellationToken)
    {
        string? currentVersion = await this.cacheRepository.GetCacheVersionAsync();
        
        if (LocalizationExtension.IsDictionaryActual(version, currentVersion))
        {
            return new VersionedListResponse<TResponse>
            {
                Items = new List<TResponse>(),
                Version = currentVersion
            };
        }
        
        var items = await this.cacheRepository.GetItemsFromCacheAsync();
    
        if (items.Count == 0)
        {
            items = await dictionaryRepository.GetListAsync(null, cancellationToken);
            await this.cacheRepository.ReinitializeDictionaryAsync(items);
            await this.cacheRepository.SetCacheVersionAsync();
        }
        
        if (string.IsNullOrEmpty(currentVersion))
        {
            currentVersion = await this.cacheRepository.GetCacheVersionAsync();
        }
    
        VersionedListResponse<TResponse> result = new VersionedListResponse<TResponse>
        {
            Items = await BuildSummitsTreeNode(
                items.Where(c => c.ParentId == null && c.IsActive), 
                items.Where(c => c.ParentId != null && c.IsActive), 
                cancellationToken),
            Version = currentVersion
        };

        return result;
    }

    private async Task<List<TResponse>> BuildSummitsTreeNode(
        IEnumerable<TEntity> mainEntities,
        IEnumerable<TEntity>? entities,
        CancellationToken cancellationToken)
    {
        var nodeList = new List<TResponse>();
    
        foreach (var entity in mainEntities)
        {
            var treeNode = await BuildSummitTreeNode(entity, entities, cancellationToken);
            nodeList.Add(treeNode);
        }
    
        return nodeList;
    }
    
    private async Task<TResponse> BuildSummitTreeNode(
        TEntity entity,
        IEnumerable<TEntity>? entities,
        CancellationToken cancellationToken)
    {
        this.entityValidator.IsEntityExist(entity);
    
        var childNodes = await GetChildNodes(entity, entities, cancellationToken);
        TResponse? node = this.mapper.Map<TEntity, TResponse>(entity);
    
        if (node != null)
        {
            node.Children = childNodes;
        }

        return node;
    }
    
    private async Task<List<TResponse>> GetChildNodes(
        TEntity entity,
        IEnumerable<TEntity>? entities,
        CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> children = this.GetChildren(entities, entity.Id);
        var childNodes = new List<TResponse>();
    
        var remainingEntities = entities?.Where(e => !children.Contains(e)).ToList();
    
        foreach (var child in children)
        {
            var childNode = await BuildSummitTreeNode(child, remainingEntities, cancellationToken);
            childNodes.Add(childNode);
        }
    
        return childNodes;
    }
    
    private IEnumerable<TEntity> GetChildren(IEnumerable<TEntity>? entities, TId parentId)
    {
        return entities?.Where(c => c.ParentId != null && c.ParentId.Equals(parentId)).ToList() ?? 
               this.dictionaryRepository.GetQueryable(c => c.ParentId != null && c.ParentId.Equals(parentId)).AsEnumerable();
    }
}