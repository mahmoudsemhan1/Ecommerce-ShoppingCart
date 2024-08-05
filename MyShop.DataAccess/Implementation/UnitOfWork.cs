using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyShop.DataAcess;
using MyShop.Entities.Models;
using MyShop.Entities.Models.ViewModels;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ICategoryRepository category { get; private set; }

        public IProductRepository product { get; private set; }
        public IShoppingCart shoppingCart { get; private set; }
		public IOrderHeaderRepository orderHeader { get; private set; }

		public IOrderDetailsRepository orderDetails { get; private set; }

		public IApplicationUserRepository ApplicationUser { get; private set; }

		public UnitOfWork(ApplicationDbConext context) 
        {
            _context = context;
            product = new ProductRepository(context);
            category = new CategoryRepository(context);
            shoppingCart = new ShoppingCart(context,_httpContextAccessor);
            orderHeader = new OrderHeaderRepository(context);
            orderDetails = new OrderDetailRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
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
