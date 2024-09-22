using AuthGateway.Domain.Models.Users;
using CommonModule.Facade;
using Microsoft.EntityFrameworkCore;

namespace AuthGateway.Domain;

public class AuthDataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public AuthDataContext(DbContextOptions<AuthDataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users", "Users");
            entity.HasMany(u => u.Roles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles", "Users");
            entity.HasMany(r => r.Users)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles", "Users");
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });
        });

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }
}