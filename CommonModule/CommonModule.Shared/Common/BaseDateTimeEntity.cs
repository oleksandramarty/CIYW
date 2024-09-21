namespace CommonModule.Shared.Common;

public class BaseDateTimeEntity<TId>: BaseIdEntity<TId>
{
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}