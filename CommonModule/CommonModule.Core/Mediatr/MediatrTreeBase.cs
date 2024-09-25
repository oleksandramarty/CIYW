using AutoMapper;
using CommonModule.Interfaces;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Core.Mediatr;

public class MediatrTreeBase<TDataContext, TEntity, TResponse, TId, TParent>
    where TEntity : class, ITreeEntity<TId, TParent>
    where TResponse : class, ITreeChildren<TResponse>
    where TDataContext : DbContext
{
    private readonly IMapper mapper;
    private readonly IEntityValidator<TDataContext> entityValidator;
    private readonly IReadGenericRepository<TId, TEntity, TDataContext> repository;

    public MediatrTreeBase(
        IMapper mapper, 
        IEntityValidator<TDataContext> entityValidator, 
        IReadGenericRepository<TId, TEntity, TDataContext> repository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.repository = repository;
    }
    
    protected async Task<List<TreeNodeResponse<TResponse>>> BuildSummitsTreeNode(
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

    protected async Task<TreeNodeResponse<TResponse>> BuildSummitTreeNode(
        TEntity entity,
        IEnumerable<TEntity>? entities,
        CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateExist(entity, entity.Id);

        var childNodes = await GetChildNodes(entity, entities, cancellationToken);
        TEntity? parent = await this.GetParentAsync(entities, entity.ParentId, cancellationToken);
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
    
    protected async Task<List<TResponse>> BuildListNodeResponse(
        TEntity entity,
        IEnumerable<TEntity>? entities)
    {
        this.entityValidator.ValidateExist(entity, entity.Id);
        
        IEnumerable<TEntity> children = this.GetChildren(entities, entity.Id);
        var responseList = new List<TResponse>();

        foreach (var child in children)
        {
            var childResponse = this.mapper.Map<TEntity, TResponse>(child);
            responseList.Add(childResponse);
        }

        return responseList;
    }
    
    protected async Task<List<TEntity>> BuildListNode(
        TEntity entity,
        IEnumerable<TEntity>? entities)
    {
        this.entityValidator.ValidateExist(entity, entity.Id);
        
        var responseList = new List<TEntity> { entity };

        IEnumerable<TEntity> children = this.GetChildren(entities, entity.Id);

        foreach (var child in children)
        {
            var childList = await BuildListNode(child, entities);
            responseList.AddRange(childList);
        }

        return responseList;
    }
    
    private IEnumerable<TEntity> GetChildren(IEnumerable<TEntity>? entities, TId parentId)
    {
        return entities?.Where(c => c.ParentId != null && c.ParentId.Equals(parentId)).ToList() ?? 
               this.repository.GetQueryable(c => c.ParentId != null && c.ParentId.Equals(parentId)).AsEnumerable();
    }
    
    private async Task<TEntity?> GetParentAsync(IEnumerable<TEntity>? entities, TParent parentId, CancellationToken cancellationToken)
    {
        if (parentId == null)
        {
            return null;
        }
        
        return entities != null ? 
            entities.FirstOrDefault(c => c.Id != null && c.Id.Equals(parentId)) :
            await this.repository.GetByIdAsync((TId)(object)parentId, cancellationToken);
    }
}