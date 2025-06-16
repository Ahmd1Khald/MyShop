using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implementaions
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public ProductRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void update(Product category)
        {
            var categoryInDB = _dbContext.Products.FirstOrDefault(x=>x.Id == category.Id);
            if (categoryInDB != null)
            {
                categoryInDB.Name = category.Name;
                categoryInDB.Description = category.Description;
                categoryInDB.Price = category.Price;
                categoryInDB.Image = category.Image;
            }
        }
    }
}
