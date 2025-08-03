using Microsoft.EntityFrameworkCore;
using APIVault.API.Models;

namespace APIVault.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<ApiKeyScope> ApiKeyScopes { get; set; }
        public DbSet<GroupApiScope> GroupApiScopes { get; set; }
        public DbSet<ApiAuditLog> ApiAuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== User Config =====
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // ===== ApiKey Config =====
            modelBuilder.Entity<ApiKey>()
                .Property(a => a.Key)
                .IsRequired();

            modelBuilder.Entity<ApiKey>()
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<ApiKey>()
                .Property(a => a.IsRevoked)
                .HasDefaultValue(false);

            modelBuilder.Entity<ApiKey>()
                .Property(a => a.ExpiresAt)
                .IsRequired(false);

            // ===== ApiKeyScope: Composite Key =====
            modelBuilder.Entity<ApiKeyScope>()
                .HasKey(aks => new { aks.ApiKeyId, aks.ApiScopeId });

            modelBuilder.Entity<ApiKeyScope>()
                .HasOne(aks => aks.ApiKey)
                .WithMany(k => k.ApiKeyScopes)
                .HasForeignKey(aks => aks.ApiKeyId);

            modelBuilder.Entity<ApiKeyScope>()
                .HasOne(aks => aks.ApiScope)
                .WithMany(s => s.ApiKeyScopes)
                .HasForeignKey(aks => aks.ApiScopeId);

            // ===== GroupApiScope: Composite Key =====
            modelBuilder.Entity<GroupApiScope>()
                .HasKey(gas => new { gas.GroupId, gas.ApiScopeId });

            modelBuilder.Entity<GroupApiScope>()
                .HasOne(gas => gas.Group)
                .WithMany(g => g.GroupApiScopes)
                .HasForeignKey(gas => gas.GroupId);

            modelBuilder.Entity<GroupApiScope>()
                .HasOne(gas => gas.ApiScope)
                .WithMany(s => s.GroupApiScopes)
                .HasForeignKey(gas => gas.ApiScopeId);

            // ===== ApiScope Config =====
            modelBuilder.Entity<ApiScope>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<ApiScope>()
                .Property(a => a.Description)
                .HasMaxLength(255);

            // ===== Role Config =====
            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            // ===== Group Config =====
            modelBuilder.Entity<Group>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<ApiAuditLog>()
    .HasOne(log => log.User)
    .WithMany()
    .HasForeignKey(log => log.UserId)
    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
