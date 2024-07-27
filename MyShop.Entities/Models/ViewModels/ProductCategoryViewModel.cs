using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Models.ViewModels
{
    public class ProductCategoryViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public Product Product { get; set; }
        public ProductVm ProductVm { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public Category Category { get; set; }

    }
}
