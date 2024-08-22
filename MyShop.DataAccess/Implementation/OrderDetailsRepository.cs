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
    public class OrderDetailRepository : GenericRepositiory<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;

		public OrderDetailRepository(ApplicationDbContext context):base(context) 
		{
			_context = context;
		}

		public void update(OrderDetails orderDetails)
		{
			_context.OrderDetails.Update(orderDetails);
		}
	}
}
