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
    public class CategoryRepository : GenericRepositiory<Category>,ICategoryRepository
    {
        private readonly ApplicationDbConext _context;

       

        public CategoryRepository(ApplicationDbConext context) : base(context)
        {
            _context = context;

        }

        public void update(Category category)
        {
            var categoryInDb=_context.Categories.FirstOrDefault(x=>x.Id==category.Id);
            if (categoryInDb != null)
            {
                categoryInDb.Name=category.Name;
                categoryInDb.Description=category.Description;
                categoryInDb.CreatedTime=DateTime.Now;
            }

        }
    }
}
