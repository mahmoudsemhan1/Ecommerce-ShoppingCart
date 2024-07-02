using Microsoft.EntityFrameworkCore;
using MyShop.Web.Models;

namespace MyShop.Web.Data
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
