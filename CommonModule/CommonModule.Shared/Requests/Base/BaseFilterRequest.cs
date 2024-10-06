using CommonModule.Shared.Common;

namespace CommonModule.Shared.Requests.Base;

public class BaseFilterRequest
{
    public PaginatorEntity? Paginator { get; set; }
    public BaseSortableRequest? Sort { get; set; }
    public BaseDateRangeFilterRequest? DateRange { get; set; }
    public BaseAmountRangeFilterRequest? AmountRange { get; set; }
    
    public string? Query { get; set; }
}