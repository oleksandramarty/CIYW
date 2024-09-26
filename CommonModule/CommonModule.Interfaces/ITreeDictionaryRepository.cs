using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Interfaces;

public interface ITreeDictionaryRepository<TId, TParentId, TEntity, TResponse, TDataContext>
    where TEntity : class, ITreeEntity<TId, TParentId>, IActivatable
    where TResponse : class, ITreeChildren<TResponse>
    where TDataContext : DbContext
{
    Task<VersionedList<TreeNodeResponse<TResponse>>> GetTreeDictionaryAsync(CancellationToken cancellationToken);
}