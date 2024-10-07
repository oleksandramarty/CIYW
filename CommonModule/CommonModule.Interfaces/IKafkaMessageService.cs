using CommonModule.Shared.Domain.AuditTrail;

namespace CommonModule.Interfaces;

public interface IKafkaMessageService
{
    Task LogAuditTrailAsync(AuditTrailLog log);
}