using CommonModule.Shared.Enums.AuditTrail;

namespace CommonModule.Shared.Domain.AuditTrail;

public class AuditTrailLog
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime Date { get; set; }
    public AuditTrailEntityEnum Entity { get; set; }
    public string EntityId { get; set; }
    public AuditTrailMethodEnum Method { get; set; }
    public string Uri { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}