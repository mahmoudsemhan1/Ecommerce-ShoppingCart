using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyShop.Entities.Models;

namespace MyShop.DataAcess
{
    public class ApplicationDbConext:IdentityDbContext<IdentityUser>
    //<IdentityUser> this mean i will extend Identityuser not add new model
    {
        public ApplicationDbConext(DbContextOptions<ApplicationDbConext> options):
            base(options)
        {
      
        }
        public DbSet<Category> Categories  { get; set; }
        public DbSet<Product> products  { get; set; }
        public DbSet<ApplicationUser> applicationUsers  { get; set; }
    }
}
