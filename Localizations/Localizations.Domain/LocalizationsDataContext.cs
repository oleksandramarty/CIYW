using Localizations.Domain.Models.Locales;
using Microsoft.EntityFrameworkCore;

namespace Localizations.Domain;

public class LocalizationsDataContext: DbContext
{
    public DbSet<Locale> Locales { get; set; }
    public DbSet<Localization> Localizations { get; set; }
    
    
    public LocalizationsDataContext(DbContextOptions<LocalizationsDataContext> options)
        : base(options)
    {
    }

    // Overriding the OnModelCreating method to configure the model
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuring the Contact entity to map to the "Contacts.Contact" table
        modelBuilder.Entity<Locale>(entity => 
        { 
            entity.ToTable("Locales", "Locales"); 
        });
        modelBuilder.Entity<Localization>(entity => 
        { 
            entity.ToTable("Localizations", "Locales"); 
        });
        
        modelBuilder.Entity<Localization>()
            .HasIndex(l => new { l.LocaleId, l.Key })
            .IsUnique();

        // Changing cascade delete behavior to restrict delete for all foreign keys
        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }
}