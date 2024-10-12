using AuthGateway.Domain.Models.Users;
using CommonModule.Facade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AuthGateway.Domain;

public class AuthGatewayDataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserSetting> UserSettings { get; set; }

    public AuthGatewayDataContext(DbContextOptions<AuthGatewayDataContext> options)
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
            entity.HasOne(u => u.UserSetting)
                .WithOne(us => us.User)
                .HasForeignKey<UserSetting>(us => us.UserId);
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
        
        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.ToTable("UserSettings", "Users");
        });

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(modelBuilder);
    }
}

public class AuthGatewayDataContextFactory : IDesignTimeDbContextFactory<AuthGatewayDataContext>
{
    public AuthGatewayDataContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json", optional: true);

        var configuration = configurationBuilder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<AuthGatewayDataContext>();
        var connectionString = configuration.GetConnectionString("Database");
        optionsBuilder.UseNpgsql(connectionString);

        return new AuthGatewayDataContext(optionsBuilder.Options);
    }
}