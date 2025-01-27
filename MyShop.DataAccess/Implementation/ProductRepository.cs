﻿using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
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
        public Product GetById(int id)
        {
            var ProCat = _context.products
                .Include(c => c.category)
                .SingleOrDefault(x => x.Id == id);
            return ProCat;

        }
    }
}
