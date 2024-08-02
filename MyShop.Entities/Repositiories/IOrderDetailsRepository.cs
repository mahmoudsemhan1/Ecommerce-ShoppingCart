﻿using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositiories
{
    public interface IOrderDetailsRepository : IGenericRepositiory<OrderDetails>
    {
        void update(OrderDetails orderDetails);







    }
}
