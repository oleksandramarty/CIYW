using AuditTrail.Domain;
using AuditTrail.Domain.Models;
using CommonModule.Interfaces;
using CommonModule.Shared.Enums.AuditTrail;

namespace AuditTrail.Business;

/// <summary>
/// Repository for AuditTrail
/// </summary>
public class AuditTrailRepository: IAuditTrailRepository
{
    private readonly IGenericRepository<Guid, AuditTrailEntity, AuditTrailDataContext> auditTrailRepository;

    public AuditTrailRepository(
        IGenericRepository<Guid, AuditTrailEntity, AuditTrailDataContext> auditTrailRepository
        )
    {
        this.auditTrailRepository = auditTrailRepository;
    }
    
    /// <summary>
    /// Add log to the database
    /// </summary>
    /// <param name="auditLog">Audit log entity</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public async Task AddLogAsync(AuditTrailEntity auditLog, CancellationToken cancellationToken)
    {
        await this.auditTrailRepository.AddAsync(auditLog, cancellationToken);
    }

    /// <summary>
    /// Add log to the database
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="exceptionType">Exception type</param>
    /// <param name="message">Exception message</param>
    /// <param name="payload">Request payload</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public async Task AddExceptionLogAsync(
        Guid? userId,
        ExceptionTypeEnum exceptionType,
        string? message,
        string? payload,
        CancellationToken cancellationToken)
    {
        await this.auditTrailRepository.AddAsync(new AuditTrailEntity
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Type = AuditTrailTypeEnum.Error,
            ExceptionType = exceptionType,
            Message = message,
            Payload = payload,
            UserId = userId
        }, cancellationToken);
    }
}