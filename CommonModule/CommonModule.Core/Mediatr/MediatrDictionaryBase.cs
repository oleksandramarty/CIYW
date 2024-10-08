using CommonModule.Interfaces;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Core.Mediatr;

public class MediatrDictionaryBase<TRequest, TId, TEntity, TResponse, TDataContext>: IRequestHandler<TRequest, VersionedList<TResponse>>
    where TRequest : IBaseVersionEntity, IRequest<VersionedList<TResponse>>
    where TEntity : class, IBaseIdEntity<TId>, IActivatable
    where TResponse : class, IBaseIdEntity<TId>
    where TDataContext : DbContext
{
    private readonly IDictionaryRepository<TId, TEntity, TResponse, TDataContext> dictionaryRepository;
    
    public MediatrDictionaryBase(IDictionaryRepository<TId, TEntity, TResponse, TDataContext> dictionaryRepository)
    {
        this.dictionaryRepository = dictionaryRepository;
    }
    
    public async Task<VersionedList<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return await this.dictionaryRepository.GetDictionaryAsync(request.Version, cancellationToken);
    }
}