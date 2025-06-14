using Microsoft.EntityFrameworkCore;
using MyShop.Web.Models;

namespace MyShop.Web.Date
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category>Categories { get; set; }
    }
}
