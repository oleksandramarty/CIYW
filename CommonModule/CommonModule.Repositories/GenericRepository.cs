using System.Linq.Expressions;
using AutoMapper;
using CommonModule.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Repositories;

public class GenericRepository<TId, T, TDataContext> : IGenericRepository<TId, T, TDataContext>
    where T : class
    where TDataContext : DbContext
{
    private readonly TDataContext dataContext;
    private readonly DbSet<T> dbSet;
    private readonly IMapper mapper;

    public GenericRepository(
        TDataContext dataContext,
        IMapper mapper)
    {
        this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        this.dbSet = this.dataContext.Set<T>();
        this.mapper = mapper;
    }

    public async Task<T> GetByIdAsync(TId id, CancellationToken cancellationToken, params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs)
    {
        IQueryable<T> query = dbSet;

        if (includeFuncs != null)
        {
            foreach (var includeFunc in includeFuncs)
            {
                query = includeFunc(query);
            }            
        }

        T entity = await query.FirstOrDefaultAsync(e => EF.Property<TId>(e, "Id").Equals(id), cancellationToken);
        return entity;
    }
    
    public async Task<T> GetAsync(Expression<Func<T, bool>> condition, CancellationToken cancellationToken,
        params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs)
    {
        IQueryable<T> query = dbSet;

        if (includeFuncs != null)
        {
            foreach (var includeFunc in includeFuncs)
            {
                query = includeFunc(query);
            }            
        }

        T entity = await query.FirstOrDefaultAsync(condition, cancellationToken);
        return entity;
    }
    
    public async Task<List<T>> GetListAsync(
        Expression<Func<T, bool>>? condition,  
        CancellationToken cancellationToken,
        params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs)
    {
        IQueryable<T> query = dbSet;
        
        if (includeFuncs != null)
        {
            foreach (var includeFunc in includeFuncs)
            {
                query = includeFunc(query);
            }            
        }
        
        List<T> entities = await (condition == null ? query : query.Where(condition)).ToListAsync(cancellationToken);
        return entities;
    }
    
    public IQueryable<T> GetQueryable(
        Expression<Func<T, bool>>? condition,
        params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs)
    {
        IQueryable<T> query = dbSet;
        
        if (includeFuncs != null)
        {
            foreach (var includeFunc in includeFuncs)
            {
                query = includeFunc(query);
            }            
        }
        
        return condition == null ? query : query.Where(condition);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await this.dbSet.AddAsync(entity, cancellationToken);
        await this.dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        this.dataContext.Entry(entity).State = EntityState.Modified;
        await this.dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TId id, CancellationToken cancellationToken)
    {
        var entity = await this.GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
            this.dbSet.Remove(entity);
            await this.dataContext.SaveChangesAsync(cancellationToken);
        }
    }
    
    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        await this.dbSet.AddRangeAsync(entities, cancellationToken);
        await this.dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
        {
            this.dataContext.Entry(entity).State = EntityState.Modified;
        }
        await this.dataContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        this.dbSet.RemoveRange(entities);
        await this.dataContext.SaveChangesAsync(cancellationToken);
    }

public async Task RemoveByIdAsync(TId id, CancellationToken cancellationToken)
{
    var entity = await this.GetByIdAsync(id, cancellationToken);
    if (entity != null)
    {
        this.dbSet.Remove(entity);
        await this.dataContext.SaveChangesAsync(cancellationToken);
    }
}
}