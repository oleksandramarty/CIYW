using CommonModule.Shared.Common;

namespace CommonModule.Shared.Responses.Base;

public class ListWithIncludeResponse<TResponse>
{
    public ListWithIncludeResponse()
    {
        
    }
    
    public ListWithIncludeResponse(IList<TResponse> entities, PaginatorEntity paginator, long totalCount)
    {
        Entities = entities;
        Paginator = paginator;
        TotalCount = totalCount;
    }

    public IList<TResponse> Entities { get; set; }
    public PaginatorEntity? Paginator { get; set; }
    public long TotalCount { get; set; }
}