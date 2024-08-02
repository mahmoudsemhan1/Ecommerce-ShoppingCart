using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositiories
{
    public interface IProductRepository:IGenericRepositiory<Product>
    {
        void update(Product product);

        Product GetById(int id);




    }
}
