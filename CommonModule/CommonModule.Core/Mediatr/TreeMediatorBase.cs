using AutoMapper;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Core.Mediatr;

public class TreeMediatorBase<TDataContext, TEntity, TResponse, TId, TParent>
    where TEntity : class, ITreeEntity<TId, TParent>
    where TResponse : class
    where TDataContext : DbContext
{
    protected readonly IMapper mapper;
    protected readonly IEntityValidator<TDataContext> entityValidator;
    protected readonly IGenericRepository<TId, TEntity, TDataContext> repository;

    public TreeMediatorBase(
        IMapper mapper, 
        IEntityValidator<TDataContext> entityValidator, 
        IGenericRepository<TId, TEntity, TDataContext> repository)
    {
        this.mapper = mapper;
        this.entityValidator = entityValidator;
        this.repository = repository;
    }

    protected async Task<TreeNodeResponse<TResponse>> BuildTreeNode(
        TEntity entity, 
        CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateExist(entity, entity != null ? entity.Id : default(TId));
        
        var children = await this.repository
            .GetListAsync(l => l.ParentId != null && l.ParentId.Equals(entity.Id), cancellationToken);
        var childNodes = new List<TreeNodeResponse<TResponse>>();

        foreach (var child in children)
        {
            var childNode = await BuildTreeNode(child, cancellationToken);
            childNodes.Add(childNode);
        }

        TEntity? parent = entity.ParentId != null ?
            await this.repository.GetByIdAsync((TId)(object)entity.ParentId, cancellationToken) :
            null;

        return new TreeNodeResponse<TResponse>
        {
            Node = this.mapper.Map<TEntity, TResponse>(entity),
            Parent = parent != null ? this.mapper.Map<TEntity, TResponse>(parent) : null,
            Children = childNodes
        };
    }
    
    protected async Task<List<TResponse>> BuildListNodeResponse(
        TEntity entity,
        CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateExist(entity, entity != null ? entity.Id : default(TId));
        
        var children = await this.repository
            .GetListAsync(l => l.ParentId != null && l.ParentId.Equals(entity.Id), cancellationToken);
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
        CancellationToken cancellationToken)
    {
        this.entityValidator.ValidateExist(entity, entity != null ? entity.Id : default(TId));
        
        var responseList = new List<TEntity> { entity };

        var children = await this.repository
            .GetListAsync(l => l.ParentId != null && l.ParentId.Equals(entity.Id), cancellationToken);

        foreach (var child in children)
        {
            var childList = await BuildListNode(child, cancellationToken);
            responseList.AddRange(childList);
        }

        return responseList;
    }
}