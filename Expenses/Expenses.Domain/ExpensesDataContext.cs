using CommonModule.Core;
using CommonModule.Facade;
using CommonModule.Shared.Core;
using Expenses.Domain.Models.Balances;
using Microsoft.EntityFrameworkCore;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Expenses.Domain;

public class ExpensesDataContext : DbSaveChangeContext
{
    public DbSet<ExpenseEntity> Expenses { get; set; }
    public DbSet<PlannedExpenseEntity> PlannedExpenses { get; set; }
    public DbSet<FavoriteExpenseEntity> FavoriteExpenses { get; set; }

    public DbSet<UserProjectEntity> UserProjects { get; set; }
    public DbSet<UserAllowedProjectEntity> UserAllowedProjects { get; set; }

    public DbSet<BalanceEntity> Balances { get; set; }

    public ExpensesDataContext(DbContextOptions<ExpensesDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExpenseEntity>(entity =>
        {
            entity.ToTable("Expenses", "Expenses");
            entity.HasOne(e => e.UserProject)
                .WithMany(uc => uc.Expenses)
                .HasForeignKey(e => e.UserProjectId);
            entity.HasOne(e => e.FavoriteExpense)
                .WithMany(uc => uc.Expenses)
                .HasForeignKey(e => e.FavoriteExpenseId);
            entity.Property(c => c.Title).HasMaxLength(50);
            entity.Property(c => c.Description).HasMaxLength(100);
            entity.Property(c => c.Amount).IsRequired();
            entity.Property(v => v.Version).IsRequired().HasMaxLength(32).IsFixedLength();
        });
        modelBuilder.Entity<PlannedExpenseEntity>(entity =>
        {
            entity.ToTable("PlannedExpenses", "Expenses");
            entity.HasOne(e => e.UserProject)
                .WithMany(uc => uc.PlannedExpenses)
                .HasForeignKey(e => e.UserProjectId);
            entity.Property(c => c.Title).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Description).HasMaxLength(100);
            entity.Property(c => c.Amount).IsRequired();
            entity.Property(v => v.Version).IsRequired().HasMaxLength(32).IsFixedLength();
        });
        modelBuilder.Entity<FavoriteExpenseEntity>(entity =>
        {
            entity.ToTable("FavoriteExpenses", "Expenses");
            entity.HasOne(e => e.UserProject)
                .WithMany(uc => uc.FavoriteExpenses)
                .HasForeignKey(e => e.UserProjectId);
            entity.Property(c => c.Title).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Description).HasMaxLength(100);
            entity.Property(v => v.Version).IsRequired().HasMaxLength(32).IsFixedLength();
        });
        modelBuilder.Entity<UserProjectEntity>(entity =>
        {
            entity.ToTable("UserProjects", "Projects");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status);
            entity.Property(e => e.CreatedUserId);
            entity.Property(v => v.Version).IsRequired().HasMaxLength(32).IsFixedLength();

            entity.HasMany(e => e.Balances)
                .WithOne()
                .HasForeignKey("UserProjectId");

            entity.HasMany(e => e.AllowedUsers)
                .WithOne(e => e.UserProject)
                .HasForeignKey(e => e.UserProjectId);

            entity.HasMany(e => e.Expenses)
                .WithOne(e => e.UserProject)
                .HasForeignKey(e => e.UserProjectId);

            entity.HasMany(e => e.PlannedExpenses)
                .WithOne(e => e.UserProject)
                .HasForeignKey(e => e.UserProjectId);

            entity.HasMany(e => e.FavoriteExpenses)
                .WithOne(e => e.UserProject)
                .HasForeignKey(e => e.UserProjectId);
        });

        modelBuilder.Entity<UserAllowedProjectEntity>(entity =>
        {
            entity.ToTable("UserAllowedProjects", "Projects");
            entity.HasOne(e => e.UserProject)
                .WithMany(uc => uc.AllowedUsers)
                .HasForeignKey(e => e.UserProjectId);
            entity.Property(v => v.Version).IsRequired().HasMaxLength(32).IsFixedLength();
        });

        modelBuilder.Entity<BalanceEntity>(entity =>
        {
            entity.ToTable("Balances", "Balance");
            entity.Property(v => v.Version).IsRequired().HasMaxLength(32).IsFixedLength();
            entity.Property(b => b.Title).HasMaxLength(50);
        });

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }
}

public class ExpensesDataContextFactory : IDesignTimeDbContextFactory<ExpensesDataContext>
{
    public ExpensesDataContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json", optional: true);

        var configuration = configurationBuilder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<ExpensesDataContext>();
        var connectionString = configuration.GetConnectionString("Database");
        optionsBuilder.UseNpgsql(connectionString);

        return new ExpensesDataContext(optionsBuilder.Options);
    }
}