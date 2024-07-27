using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositiories
{
    public interface ICategoryRepository:IGenericRepositiory<Category>
    {
        void update(Category category);
        Product GetById(int id);

        IEnumerable<Category> GetAllCategories();





    }
}
