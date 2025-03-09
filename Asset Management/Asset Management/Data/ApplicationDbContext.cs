using Asset_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Asset_Management.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users, Roles, string>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetMaintenance> AssetMaintenances { get; set; }
        public DbSet<AssetMovement> AssetMovements { get; set; }
        public DbSet<AssetCheck> AssetChecks { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasIndex(c => c.CategoryName).IsUnique();
            modelBuilder.Entity<Location>().HasIndex(l => l.LocationName).IsUnique();
            modelBuilder.Entity<Status>().HasIndex(s => s.StatusName).IsUnique();
            modelBuilder.Entity<Asset>().HasIndex(a => a.AssetCode).IsUnique();

            modelBuilder.Entity<Asset>()
                .HasOne(a => a.Category)
                .WithMany()
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Asset>()
                .HasOne(a => a.Status)
                .WithMany()
                .HasForeignKey(a => a.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Asset>()
                .HasOne(a => a.Location)
                .WithMany()
                .HasForeignKey(a => a.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssetMaintenance>()
                .HasOne(am => am.Asset)
                .WithMany()
                .HasForeignKey(am => am.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssetMovement>()
                .HasOne(am => am.Asset)
                .WithMany()
                .HasForeignKey(am => am.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssetMovement>()
                .HasOne(am => am.FromLocation)
                .WithMany()
                .HasForeignKey(am => am.FromLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssetMovement>()
                .HasOne(am => am.ToLocation)
                .WithMany()
                .HasForeignKey(am => am.ToLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssetCheck>()
                .HasOne(ac => ac.Asset)
                .WithMany()
                .HasForeignKey(ac => ac.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AssetCheck>()
                .HasOne(ac => ac.Location)
                .WithMany()
                .HasForeignKey(ac => ac.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssetCheck>()
                .HasOne(ac => ac.Status)
                .WithMany()
                .HasForeignKey(ac => ac.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Roles
            var adminRole = new Roles { Id = "1", Name = "Admin", NormalizedName = "ADMIN" };
            var userRole = new Roles { Id = "2", Name = "User", NormalizedName = "USER" };

            modelBuilder.Entity<Roles>().HasData(adminRole, userRole);

            // Seed Users
            var hasher = new PasswordHasher<Users>();
            var adminUser = new Users
            {
                Id = "1",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                FullName = "Administrator",
                PasswordHash = hasher.HashPassword(null, "Admin@123")
            };

            var normalUser = new Users
            {
                Id = "2",
                UserName = "user",
                NormalizedUserName = "USER",
                Email = "user@example.com",
                NormalizedEmail = "USER@EXAMPLE.COM",
                EmailConfirmed = true,
                FullName = "Regular User",
                PasswordHash = hasher.HashPassword(null, "User@123")
            };

            modelBuilder.Entity<Users>().HasData(adminUser, normalUser);

            // Seed User Roles (Assign roles to users)
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "1", RoleId = "1" }, // Admin user assigned Admin role
                new IdentityUserRole<string> { UserId = "2", RoleId = "2" }  // Normal user assigned User role
            );
        }
    }
}
