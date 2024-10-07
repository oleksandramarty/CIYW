using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Common;

public class BaseDateTimeEntity<TId>: BaseIdEntity<TId>, IBaseDateTimeEntity<TId>
{
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}