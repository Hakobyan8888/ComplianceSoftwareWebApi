using ComplianceSoftwareWebApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComplianceSoftwareWebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ComplianceRequirement> ComplianceRequirements { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<IndustryType> IndustryTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPermission>()
            .HasOne(up => up.User)
            .WithMany(u => u.UserPermissions)
            .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany() // Assuming Permission does not have a collection of UserPermissions
                .HasForeignKey(up => up.PermissionId);

            modelBuilder.Entity<Company>()
                .HasMany(c => c.Documents)
                .WithOne(u => u.Company)
                .HasForeignKey(p => p.CompanyId)
                .HasPrincipalKey(c => c.Id);

            modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CompanyId)
            .HasPrincipalKey(c => c.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
