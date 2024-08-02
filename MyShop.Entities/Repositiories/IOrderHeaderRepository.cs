using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositiories
{
    public interface IOrderHeaderRepository : IGenericRepositiory<OrderHeader>
    {
        void update(OrderHeader orderHeader);

        void UpdateOrderStatus(int id, string Orderstatus,string PaymentStatus);






    }
}
