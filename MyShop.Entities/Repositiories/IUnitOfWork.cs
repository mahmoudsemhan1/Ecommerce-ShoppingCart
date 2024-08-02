using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositiories
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository category {  get; }
        IProductRepository product { get; }

        IShoppingCart shoppingCart { get; }
        IOrderDetailsRepository orderDetails { get; }

        IOrderHeaderRepository  orderHeader { get; }
        IApplicationUserRepository  ApplicationUser { get; }

        
         

        int Complete();

    }
}
