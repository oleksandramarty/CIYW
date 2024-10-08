using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Interfaces;

public interface IDictionaryRepository<TId, TEntity, TResponse, TDataContext>
    where TEntity : class, IBaseIdEntity<TId>, IActivatable
    where TResponse : class, IBaseIdEntity<TId>
    where TDataContext : DbContext
{
    Task<VersionedListResponse<TResponse>> GetDictionaryAsync(string? version, CancellationToken cancellationToken);
}