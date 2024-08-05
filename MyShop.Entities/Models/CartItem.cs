using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }  // Foreign key for Cart
        public Cart Cart { get; set; }   // Navigation property to Cart

        public string ApplicationUserId { get; set; }
        public Product Product { get; set; }
    }
}
