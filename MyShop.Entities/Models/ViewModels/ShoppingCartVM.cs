using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<CartItem> CartItemsList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public decimal TotalCarts { get; set; }

	}
}
