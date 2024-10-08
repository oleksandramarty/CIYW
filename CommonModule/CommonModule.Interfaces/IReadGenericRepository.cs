using System.Linq.Expressions;
using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Interfaces;

public interface IReadGenericRepository<TId, T, TDataContext>
    where T : class
    where TDataContext : DbContext
{
    Task<T> GetByIdAsync(TId id, CancellationToken cancellationToken,
        params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs);
    Task<T> GetAsync(Expression<Func<T, bool>> condition,  CancellationToken cancellationToken,
        params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs);
    Task<List<T>> GetListAsync(
        Expression<Func<T, bool>>? condition,  
        CancellationToken cancellationToken,
        params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs);
    
    IQueryable<T> GetQueryable(
        Expression<Func<T, bool>>? condition,
        params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs);

    Task RemoveByIdAsync(TId id, CancellationToken cancellationToken);
}