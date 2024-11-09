using AutoMapper;
using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Core.Strategies.GetFilteredResult;

public class GetFilteredResultStrategyResponseContext<TGetFilteredRequest, TEntity, TResponse>
    where TGetFilteredRequest: IBaseFilterRequest
{
    private readonly IMapper mapper;
    
    public GetFilteredResultStrategyResponseContext(IMapper mapper)
    {
        this.mapper = mapper;
    }
    
    protected async Task<FilteredListResponse<TResponse>> GetFilteredResultAsync(
        TGetFilteredRequest request,
        IQueryable<TEntity> query, 
        CancellationToken cancellationToken)
    {
        var total = await query.CountAsync(cancellationToken);

        List<TEntity> entities = new List<TEntity>();

        if (request.Paginator != null)
        {
            entities = await query
                .Skip((request.Paginator.PageNumber - 1) * request.Paginator.PageSize)
                .Take(request.Paginator.PageSize)
                .ToListAsync(cancellationToken);
        }
        else
        {
            entities = await query.ToListAsync(cancellationToken);
        }
        
        return new FilteredListResponse<TResponse>
        {
            Entities = entities.Select(x => this.mapper.Map<TEntity, TResponse>(x)).ToList(),
            Paginator = request?.Paginator,
            TotalCount = total
        };
    }
}