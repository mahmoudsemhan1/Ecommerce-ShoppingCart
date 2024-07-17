using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositiories
{
    public interface IGenericRepositiory<T> where T : class
    {
        //_context.Ctegories.Include("Products").Tolist();
        //_context.Ctegories.where(x=>x.Id==id).Tolist();

        IEnumerable<T> GetAll(Expression<Func<T, bool>>? perdicate=null, string? IncludeWord = null);
        //_context.Ctegories.Include("Products").Tolist();
        //_context.Ctegories.where(x=>x.Id==id).Tolist();

        T GetFirstOrDefualt(Expression<Func<T, bool>>? perdicate = null, string? IncludeWord = null);
        //_context.Ctegories.Add(Category);

        void Add(T entity);
        //_context.Ctegories.Remove(Category);
        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);









    }
}
