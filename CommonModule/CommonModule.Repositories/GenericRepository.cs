using System.Linq.Expressions;
using CommonModule.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Repositories;

public class GenericRepository<TEntityId, TEntity, TDataContext> : IGenericRepository<TEntityId, TEntity, TDataContext>
    where TEntity : class
    where TDataContext : DbContext
{
    private readonly TDataContext dataContext;
    private readonly DbSet<TEntity> dbSet;

    public GenericRepository(
        TDataContext dataContext
    )
    {
        this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        this.dbSet = this.dataContext.Set<TEntity>();
    }

    public async Task<TEntity> ByIdAsync(TEntityId id, CancellationToken cancellationToken,
        params Func<IQueryable<TEntity>, IQueryable<TEntity>>[]? includeFuncs)
    {
        IQueryable<TEntity> query = dbSet;

        if (includeFuncs != null)
        {
            foreach (var includeFunc in includeFuncs)
            {
                query = includeFunc(query);
            }
        }

        TEntity entity = await query.FirstOrDefaultAsync(e => EF.Property<TEntity>(e, "Id").Equals(id), cancellationToken);
        return entity;
    }

    public async Task<TEntity> Async(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken,
        params Func<IQueryable<TEntity>, IQueryable<TEntity>>[]? includeFuncs)
    {
        IQueryable<TEntity> query = dbSet;

        if (includeFuncs != null)
        {
            foreach (var includeFunc in includeFuncs)
            {
                query = includeFunc(query);
            }
        }

        TEntity entity = await query.FirstOrDefaultAsync(condition, cancellationToken);
        return entity;
    }

    public async Task<List<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? condition,
        CancellationToken cancellationToken,
        params Func<IQueryable<TEntity>, IQueryable<TEntity>>[]? includeFuncs)
    {
        IQueryable<TEntity> query = dbSet;

        if (includeFuncs != null)
        {
            foreach (var includeFunc in includeFuncs)
            {
                query = includeFunc(query);
            }
        }

        List<TEntity> entities = await (condition == null ? query : query.Where(condition)).ToListAsync(cancellationToken);
        return entities;
    }

    public IQueryable<TEntity> Queryable(
        Expression<Func<TEntity, bool>>? condition,
        params Func<IQueryable<TEntity>, IQueryable<TEntity>>[]? includeFuncs)
    {
        IQueryable<TEntity> query = dbSet;

        if (includeFuncs != null)
        {
            foreach (var includeFunc in includeFuncs)
            {
                query = includeFunc(query);
            }
        }

        return condition == null ? query : query.Where(condition);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await this.dbSet.AddAsync(entity, cancellationToken);
        await this.dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        this.dataContext.Entry(entity).State = EntityState.Modified;
        await this.dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        this.dataContext.Entry(entity).State = EntityState.Deleted;
        await this.dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await this.dbSet.AddRangeAsync(entities, cancellationToken);
        await this.dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
        {
            this.dataContext.Entry(entity).State = EntityState.Modified;
        }

        await this.dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        this.dbSet.RemoveRange(entities);
        await this.dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(TEntityId id, CancellationToken cancellationToken)
    {
        var entity = await this.ByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            this.dbSet.Remove(entity);
            await this.dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}