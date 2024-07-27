using Microsoft.EntityFrameworkCore;
using MyShop.DataAcess;
using MyShop.Entities.Models;
using MyShop.Entities.Repositiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implementation
{
    public class ProductRepository : GenericRepositiory<Product>,IProductRepository
    {
        private readonly ApplicationDbConext _context;
        public ProductRepository(ApplicationDbConext context) : base(context)
        {
            _context = context;

        }

        public void update(Product product)
        {
            var productInDb = _context.products.FirstOrDefault(x => x.Id == product.Id);
            if (productInDb != null)
            {
                productInDb.Name = product.Name;
                productInDb.Description = product.Description;
                productInDb.Price= product.Price;
                productInDb.Img= product.Img;     
                productInDb.CategoryId= product.CategoryId;
            }
        }
    }
}
