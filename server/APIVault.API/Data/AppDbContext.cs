using Microsoft.EntityFrameworkCore;
using APIVault.API.Models;

namespace APIVault.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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
        }
    }
}
