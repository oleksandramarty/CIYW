using Dictionaries.Domain.Models.Categories;
using Dictionaries.Domain.Models.Countries;
using Microsoft.EntityFrameworkCore;

namespace Dictionaries.Domain
{
    public class DictionariesDataContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Models.Currencies.Currency> Currencies { get; set; }
        public DbSet<CountryCurrency> CountryCurrencies { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DictionariesDataContext(DbContextOptions<DictionariesDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Countries", "Dictionaries");
                entity.HasMany(c => c.Currencies)
                      .WithOne(cc => cc.Country)
                      .HasForeignKey(cc => cc.CountryId);
            });

            modelBuilder.Entity<Models.Currencies.Currency>(entity =>
            {
                entity.ToTable("Currencies", "Dictionaries");
                entity.HasMany(c => c.Countries)
                      .WithOne(cc => cc.Currency)
                      .HasForeignKey(cc => cc.CurrencyId);
            });
            
            modelBuilder.Entity<CountryCurrency>()
                .HasIndex(l => new { l.CountryId, l.CurrencyId })
                .IsUnique();

            modelBuilder.Entity<CountryCurrency>(entity =>
            {
                entity.ToTable("CountryCurrencies", "Dictionaries");
                entity.HasKey(cc => new { cc.CountryId, cc.CurrencyId });
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories", "Dictionaries");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(255);
                entity.Property(c => c.Icon).HasMaxLength(255);
                entity.Property(c => c.Color).HasMaxLength(50);
                entity.Property(c => c.IsActive).IsRequired();
                entity.Property(c => c.IsPositive).IsRequired();
                entity.HasMany(c => c.Children)
                      .WithOne()
                      .HasForeignKey(c => c.ParentId);
            });

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }
    }
}