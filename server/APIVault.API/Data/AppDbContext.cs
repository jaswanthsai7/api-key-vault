using Microsoft.EntityFrameworkCore;
using APIVault.API.Models;

namespace APIVault.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ApiScope> ApiScopes { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<ApiKeyScope> ApiKeyScopes { get; set; }
        public DbSet<GroupApiScope> GroupApiScopes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for ApiKeyScope
            modelBuilder.Entity<ApiKeyScope>()
                .HasKey(aks => new { aks.ApiKeyId, aks.ApiScopeId });

            // Composite key for GroupApiScope
            modelBuilder.Entity<GroupApiScope>()
                .HasKey(gas => new { gas.GroupId, gas.ApiScopeId });

            // Relationships (optional but recommended)
            modelBuilder.Entity<ApiKeyScope>()
                .HasOne(aks => aks.ApiKey)
                .WithMany(k => k.ApiKeyScopes)
                .HasForeignKey(aks => aks.ApiKeyId);

            modelBuilder.Entity<ApiKeyScope>()
                .HasOne(aks => aks.ApiScope)
                .WithMany(s => s.ApiKeyScopes)
                .HasForeignKey(aks => aks.ApiScopeId);

            modelBuilder.Entity<GroupApiScope>()
                .HasOne(gas => gas.Group)
                .WithMany(g => g.GroupApiScopes)
                .HasForeignKey(gas => gas.GroupId);

            modelBuilder.Entity<GroupApiScope>()
                .HasOne(gas => gas.ApiScope)
                .WithMany(s => s.GroupApiScopes)
                .HasForeignKey(gas => gas.ApiScopeId);
        }
    }
}
