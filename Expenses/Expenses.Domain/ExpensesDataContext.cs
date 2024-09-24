using Expenses.Domain.Models.Balances;
using Microsoft.EntityFrameworkCore;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Categories;
using Expenses.Domain.Models.Projects;

namespace Expenses.Domain;

public class ExpensesDataContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<UserCategory> UserCategories { get; set; }

    public DbSet<UserProject> UserProjects { get; set; }
    public DbSet<UserAllowedProject> UserAllowedProjects { get; set; }

    public DbSet<Balance> Balances { get; set; }

    public ExpensesDataContext(DbContextOptions<ExpensesDataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("Expenses", "Expenses");
            entity.HasOne(e => e.UserCategory)
                .WithMany(uc => uc.Expenses)
                .HasForeignKey(e => e.UserCategoryId);
            entity.HasOne(e => e.UserProject)
                .WithMany(uc => uc.Expenses)
                .HasForeignKey(e => e.UserProjectId);
        });

        modelBuilder.Entity<UserCategory>(entity => { entity.ToTable("UserCategories", "Categories"); });

        modelBuilder.Entity<UserProject>(entity => { entity.ToTable("UserProjects", "Projects"); });

        modelBuilder.Entity<UserAllowedProject>(entity =>
        {
            entity.ToTable("UserAllowedProjects", "Projects");
            entity.HasOne(e => e.UserProject)
                .WithMany(uc => uc.AllowedUsers)
                .HasForeignKey(e => e.UserProjectId);
        });


        modelBuilder.Entity<Balance>(entity => { entity.ToTable("Balances", "Balance"); });

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }
}