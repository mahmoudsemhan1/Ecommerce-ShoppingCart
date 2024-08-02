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
    public class OrderHeaderRepository : GenericRepositiory<OrderHeader>,IOrderHeaderRepository
    {
        private readonly ApplicationDbConext _context;

       

        public OrderHeaderRepository(ApplicationDbConext context) : base(context)
        {
            _context = context;

        }

		public void update(OrderHeader orderHeader)
		{
			_context.OrderHeaders.Update(orderHeader);
		}

		public void UpdateOrderStatus(int id, string Orderstatus, string PaymentStatus)
		{
			var OrderFromDb=_context.OrderHeaders.FirstOrDefault(o=>o.Id == id);
			if(OrderFromDb != null)
			{
				OrderFromDb.OrderStatus=Orderstatus;
				if(PaymentStatus != null)
				{
					OrderFromDb.PaymentStatus= PaymentStatus;
				}
			}
		}
	}
}
