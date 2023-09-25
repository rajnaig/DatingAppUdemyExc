using Backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLike>()
                .HasOne(s => s.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
