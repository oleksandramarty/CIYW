namespace AuditTrail.Domain.Models;

/// <summary>
/// Entity for AuditTrail
/// </summary>
public class AuditTrailArchiveEntity: AuditTrailEntity
{
    /// <summary>
    /// ArchiveDate of the entity
    /// </summary>
    public DateTime ArchiveDate { get; set; }
}