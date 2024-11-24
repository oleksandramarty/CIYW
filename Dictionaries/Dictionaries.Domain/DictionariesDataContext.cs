using CommonModule.Core;
using CommonModule.Facade;
using CommonModule.Shared.Core;
using Dictionaries.Domain.Models.Balances;
using Dictionaries.Domain.Models.Categories;
using Dictionaries.Domain.Models.Countries;
using Dictionaries.Domain.Models.Expenses;
using Dictionaries.Domain.Models.Icons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dictionaries.Domain;

public class DictionariesDataContext : DbSaveChangeContext
{
    public DbSet<FrequencyEntity> Frequencies { get; set; }
    public DbSet<CountryEntity> Countries { get; set; }
    public DbSet<Models.Currencies.CurrencyEntity> Currencies { get; set; }
    public DbSet<CountryCurrencyEntity> CountryCurrencies { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<BalanceTypeEntity> BalanceTypes { get; set; }
    public DbSet<IconCategoryEntity> IconCategories { get; set; }
    public DbSet<IconEntity> Icons { get; set; }

    public DictionariesDataContext(DbContextOptions<DictionariesDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CountryEntity>(entity =>
        {
            entity.ToTable("Countries", "Dictionaries");
            entity.HasMany(c => c.Currencies)
                .WithOne(cc => cc.Country)
                .HasForeignKey(cc => cc.CountryId);
            entity.Property(c => c.Title).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Code).IsRequired().HasMaxLength(2).IsFixedLength();
            entity.Property(c => c.TitleEn).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Models.Currencies.CurrencyEntity>(entity =>
        {
            entity.ToTable("Currencies", "Dictionaries");
            entity.HasMany(c => c.Countries)
                .WithOne(cc => cc.Currency)
                .HasForeignKey(cc => cc.CurrencyId);
            entity.Property(c => c.Title).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Code).IsRequired().HasMaxLength(3);
            entity.Property(c => c.Symbol).IsRequired().HasMaxLength(5);
            entity.Property(c => c.TitleEn).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Status).IsRequired();
        });

        modelBuilder.Entity<CountryCurrencyEntity>()
            .HasIndex(l => new { l.CountryId, l.CurrencyId })
            .IsUnique();

        modelBuilder.Entity<CountryCurrencyEntity>(entity =>
        {
            entity.ToTable("CountryCurrencies", "Dictionaries");
            entity.HasKey(cc => new { cc.CountryId, cc.CurrencyId });
        });

        modelBuilder.Entity<FrequencyEntity>(entity =>
        {
            entity.ToTable("Frequencies", "Dictionaries");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(40);
        });

        modelBuilder.Entity<BalanceTypeEntity>(entity =>
        {
            entity.ToTable("BalanceTypes", "Dictionaries");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Type).IsRequired();
        });

        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.ToTable("Categories", "Dictionaries");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Title).IsRequired().HasMaxLength(70);
            entity.Property(c => c.Color).HasMaxLength(7).IsFixedLength().IsRequired(false);
            entity.Property(c => c.Status).IsRequired();
            entity.Property(c => c.IsPositive).IsRequired();
            entity.HasMany(c => c.Children)
                .WithOne()
                .HasForeignKey(c => c.ParentId);
        });

        modelBuilder.Entity<IconEntity>(entity =>
        {
            entity.ToTable("Icons", "Dictionaries");
            entity.HasMany(c => c.Categories)
                .WithOne(cc => cc.Icon)
                .HasForeignKey(cc => cc.IconId);
            entity.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(50);
        });
        modelBuilder.Entity<IconCategoryEntity>(entity =>
        {
            entity.ToTable("IconCategories", "Dictionaries");
            entity.HasMany(c => c.Icons)
                .WithOne(cc => cc.IconCategory)
                .HasForeignKey(cc => cc.IconCategoryId);
            entity.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(100);
        });

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }
}

public class DictionariesDataContextFactory : IDesignTimeDbContextFactory<DictionariesDataContext>
{
    public DictionariesDataContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json", optional: true);

        var configuration = configurationBuilder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<DictionariesDataContext>();
        var connectionString = configuration.GetConnectionString("Database");
        optionsBuilder.UseNpgsql(connectionString);

        return new DictionariesDataContext(optionsBuilder.Options);
    }
}