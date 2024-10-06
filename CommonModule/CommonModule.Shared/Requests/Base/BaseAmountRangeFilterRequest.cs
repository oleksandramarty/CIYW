namespace CommonModule.Shared.Requests.Base;

public class BaseAmountRangeFilterRequest
{
    public decimal? AmountFrom { get; set; }
    public decimal? AmountTo { get; set; }
}