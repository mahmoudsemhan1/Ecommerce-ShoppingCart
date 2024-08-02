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
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between CartItem and Product
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId);
           
        }
    }
}
