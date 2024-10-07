using CommonModule.Shared.Domain.AuditTrail;
using Microsoft.EntityFrameworkCore;

namespace AuditTrail.Domain;

public class AuditTrailDataContext: DbContext
{
    public DbSet<AuditTrailLog> AuditTrailLogs { get; set; }

    public AuditTrailDataContext(DbContextOptions<AuditTrailDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditTrailLog>(entity => { entity.ToTable("AuditTrailLogs", "AuditTrails"); });

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }
}