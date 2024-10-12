using Microsoft.EntityFrameworkCore;

namespace CommonModule.Interfaces;

public interface IGenericRepository<TId, T, TDataContext> : IReadGenericRepository<TId, T, TDataContext>
    where T : class
    where TDataContext : DbContext
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
    Task DeleteAsync(TId id, CancellationToken cancellationToken);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    Task RemoveByIdAsync(TId id, CancellationToken cancellationToken);
}