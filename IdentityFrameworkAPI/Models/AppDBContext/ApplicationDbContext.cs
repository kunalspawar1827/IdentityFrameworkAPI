using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityFrameworkAPI.Models.AppDBContext
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }


        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                
                 new IdentityRole() { Name = "Admin" , ConcurrencyStamp = "1" , NormalizedName = "Admin" , Id = "1"},
                 new IdentityRole() { Name = "User" , ConcurrencyStamp = "2" , NormalizedName = "User" , Id = "2"} ,
                  new IdentityRole() { Name = "HR", ConcurrencyStamp = "3", NormalizedName = "HR", Id = "3" }

                );
        }
    }
}
