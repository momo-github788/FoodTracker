using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using SuperHeroApi.Models;

namespace backend.Data {
    public class ApplicationDbContext : IdentityDbContext<User> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(u => u.FoodRecords)
                .WithOne(tr => tr.User).IsRequired();



            builder.Entity<User>().ToTable("User");
            builder.Entity<IdentityRole>().ToTable("Role");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<IdentityUserClaim<string>>().ToTable("Claim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
        }

        public DbSet<FoodRecord> FoodRecords { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
    }
}
