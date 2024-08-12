using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Models.ViewModels
{
    public class OrderVM
    {
        public OrderHeader orderHeader {  get; set; }
        public IEnumerable<OrderDetails> OrderDetails {  get; set; }
          // Optional: If you need to include ApplicationUser separately
        public ApplicationUser ApplicationUsers { get; set; }
        public Product product { get; set; }

    }
}
