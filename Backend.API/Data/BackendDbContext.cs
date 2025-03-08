using Backend.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Backend.API.Data
{
    public class BackendDbContext :IdentityDbContext<ApplicationUser>
    {
        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var userRoleId = "430f06bf-f5cd-4d94-87f5-cb9575698d74";
            var adminRoleId = "d251e4e9-a928-48da-aa5d-720eaa10789c";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "USER".ToUpper()
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasMany(u => u.Posts)
                      .WithOne(p => p.User)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Comments)
                      .WithOne(c => c.User)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Post>(entity =>
            {
                entity.HasOne(p => p.User)
                      .WithMany(u => u.Posts)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Comments)
                      .WithOne(c => c.Post)
                      .HasForeignKey(c => c.PostId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            builder.Entity<Comment>(entity =>
            {
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.Post)
                      .WithMany(p => p.Comments)
                      .HasForeignKey(c => c.PostId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
