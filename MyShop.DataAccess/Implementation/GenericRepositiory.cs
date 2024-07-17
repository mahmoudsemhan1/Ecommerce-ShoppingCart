using Microsoft.EntityFrameworkCore;
using MyShop.DataAcess;
using MyShop.Entities.Repositiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implementation
{
    public class GenericRepositiory<T> : IGenericRepositiory<T> where T : class
    {
        private readonly ApplicationDbConext _context;
        private DbSet<T> _dbset;

        public GenericRepositiory(ApplicationDbConext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public void Add(T entity)
        {
            //Categories.add(Category)
            _dbset.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? perdicate = null, string? IncludeWord = null)
        {
            IQueryable<T> query = _dbset;
            if(perdicate != null)
            {
                query = query.Where(perdicate);
            }
            if(IncludeWord != null)
            {
                //_context.Products.Include("Ctegory,Logos,Users")
                foreach(var item in IncludeWord.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefualt(Expression<Func<T, bool>>? perdicate=null, string? IncludeWord = null)
        {
            IQueryable<T> query = _dbset;
            if (perdicate != null)
            {
                query = query.Where(perdicate);
            }
            if (IncludeWord != null)
            {
                //_context.Products.Include("Ctegory,Logos,Users")
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.SingleOrDefault();
        }

        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbset.RemoveRange(entity);
        }
    }
}
