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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbConext _context;
        public ICategoryRepository category { get; private set; }
        public UnitOfWork(ApplicationDbConext context) 
        {
            _context = context;
            category = new CategoryRepository(context);
             

        }


        public int Complete()
        {
            return  _context.SaveChanges();

        }

        public void Dispose()
        {
            _context.Dispose();

        }
    }
}
