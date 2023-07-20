using Backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppUser u1 = new AppUser()
            {
                Email = "bcoronado2@yourhouselive.com",
                UserName = "Con"
            };

            modelBuilder.Entity<AppUser>().HasData(u1);

            base.OnModelCreating(modelBuilder);
        }
    }
}
