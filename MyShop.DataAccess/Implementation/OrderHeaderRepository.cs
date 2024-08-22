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
        private readonly ApplicationDbContext _context;

       

        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public IEnumerable<OrderHeader> GetAllOrderHeader()
        {
			return _context.OrderHeaders.ToList();	
		}

        public void update(OrderHeader orderHeader)
		{
            var entity = _context.OrderHeaders.Attach(orderHeader);
            entity.State = EntityState.Modified;
        }

		public void UpdateOrderStatus(int id, string Orderstatus, string PaymentStatus)
		{
			var OrderFromDb=_context.OrderHeaders.FirstOrDefault(o=>o.Id == id);
			if(OrderFromDb != null)
			{
				OrderFromDb.OrderStatus=Orderstatus;
				OrderFromDb.PaymentDate=DateTime.Now;
				if(PaymentStatus != null)
				{
					OrderFromDb.PaymentStatus= PaymentStatus;
				}
			}
		}
	}
}
