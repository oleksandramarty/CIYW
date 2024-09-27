using CommonModule.Interfaces;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Core.Mediatr;

public class MediatrTreeDictionaryBase<TRequest, TId, TParentId, TEntity, TResponse, TDataContext>: IRequestHandler<TRequest, VersionedList<TreeNodeResponse<TResponse>>>
    where TRequest : IBaseVersionEntity, IRequest<VersionedList<TreeNodeResponse<TResponse>>>
    where TEntity : class, ITreeEntity<TId, TParentId>, IActivatable
    where TResponse : class, ITreeChildren<TResponse>
    where TDataContext : DbContext
{
    private readonly ITreeDictionaryRepository<TId, TParentId, TEntity, TResponse, TDataContext> treeDictionaryRepository;
    
    public MediatrTreeDictionaryBase(ITreeDictionaryRepository<TId, TParentId, TEntity, TResponse, TDataContext> treeDictionaryRepository)
    {
        this.treeDictionaryRepository = treeDictionaryRepository;
    }
    
    public async Task<VersionedList<TreeNodeResponse<TResponse>>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return await this.treeDictionaryRepository.GetTreeDictionaryAsync(request.Count, request.Version, cancellationToken);
    }
}