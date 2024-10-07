using AutoMapper;
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
    
    public async Task<VersionedList<TreeNodeResponse<TResponse>>> GetTreeDictionaryAsync(string? version, CancellationToken cancellationToken)
    {
        string currentVesrion = await this.cacheRepository.GetCacheVersionAsync();
        
        if (
            !string.IsNullOrEmpty(version) && 
            version.Equals(currentVesrion))
        {
            return new VersionedList<TreeNodeResponse<TResponse>>
            {
                Items = new List<TreeNodeResponse<TResponse>>(),
                Version = currentVesrion
            };
        }
        
        var items = await this.cacheRepository.GetItemsFromCacheAsync();
    
        if (items.Count == 0)
        {
            items = await dictionaryRepository.GetListAsync(null, cancellationToken);
            await this.cacheRepository.ReinitializeDictionaryAsync(items);
            await this.cacheRepository.SetCacheVersionAsync();
        }
    
        return new VersionedList<TreeNodeResponse<TResponse>>
        {
            Items = await BuildSummitsTreeNode(
                items.Where(c => c.ParentId == null && c.IsActive), 
                items.Where(c => c.ParentId != null && c.IsActive), 
                cancellationToken),
            Version = currentVesrion
        };
    }

    private async Task<List<TreeNodeResponse<TResponse>>> BuildSummitsTreeNode(
        IEnumerable<TEntity> mainEntities,
        IEnumerable<TEntity>? entities,
        CancellationToken cancellationToken)
    {
        var nodeList = new List<TreeNodeResponse<TResponse>>();
    
        foreach (var entity in mainEntities)
        {
            var treeNode = await BuildSummitTreeNode(entity, entities, cancellationToken);
            nodeList.Add(treeNode);
        }
    
        return nodeList;
    }
    
    private async Task<TreeNodeResponse<TResponse>> BuildSummitTreeNode(
        TEntity entity,
        IEnumerable<TEntity>? entities,
        CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateExist(entity, entity.Id);
    
        var childNodes = await GetChildNodes(entity, entities, cancellationToken);
        TEntity? parent = await this.GeTParentIdAsync(entities, entity.ParentId, cancellationToken);
        TResponse? node = this.mapper.Map<TEntity, TResponse>(entity);
    
        if (node != null)
        {
            node.Children = childNodes;
        }
        
        return new TreeNodeResponse<TResponse>
        {
            Node = node,
            Parent = parent != null ? this.mapper.Map<TEntity, TResponse>(parent) : null
        };
    }
    
    private async Task<List<TreeNodeResponse<TResponse>>> GetChildNodes(
        TEntity entity,
        IEnumerable<TEntity>? entities,
        CancellationToken cancellationToken)
    {
        IEnumerable<TEntity> children = this.GetChildren(entities, entity.Id);
        var childNodes = new List<TreeNodeResponse<TResponse>>();
    
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
    
    private async Task<TEntity?> GeTParentIdAsync(IEnumerable<TEntity>? entities, TParentId parentId, CancellationToken cancellationToken)
    {
        if (parentId == null)
        {
            return null;
        }
        
        return entities != null ? 
            entities.FirstOrDefault(c => c.Id != null && c.Id.Equals(parentId)) :
            await this.dictionaryRepository.GetByIdAsync((TId)(object)parentId, cancellationToken);
    }
}