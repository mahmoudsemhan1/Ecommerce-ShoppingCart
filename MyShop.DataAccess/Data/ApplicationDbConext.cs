using Microsoft.EntityFrameworkCore;
using MyShop.Entities.Models;

namespace MyShop.DataAcess
{
    public class ApplicationDbConext:DbContext
    {
        public ApplicationDbConext(DbContextOptions<ApplicationDbConext> options):
            base(options)
        {
      
        }
        public DbSet<Category> Categories  { get; set; }
    }
}
