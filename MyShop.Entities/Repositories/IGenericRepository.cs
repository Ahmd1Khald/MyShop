using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        // _dbContext.Categories.Include("Products").ToList();
        // _dbContext.Categories.Where(x=>x.Id == id).ToList();
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? predicate = null, string? includeWord = null);

        // _dbContext.Categories.Include("Products").SingleOrDefualt();
        // _dbContext.Categories.Where(x=>x.Id == id).SingleOrDefualt();
        T GetFirstOrDefualt(Expression<Func<T, bool>>? predicate = null, string? includeWord = null);

        // _dbContext.Categories.Add(entity);
        void Add(T entity);

        // _dbContext.Categories.Remove(entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
