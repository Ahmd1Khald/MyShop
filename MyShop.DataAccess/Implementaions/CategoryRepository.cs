using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implementaions
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public CategoryRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void update(Category category)
        {
            var categoryInDB = _dbContext.Categories.FirstOrDefault(x=>x.Id == category.Id);
            if (categoryInDB != null)
            {
                categoryInDB.Name = category.Name;
                categoryInDB.Description = category.Description;
                categoryInDB.CreatedTime = DateTime.Now;
            }
        }
    }
}
