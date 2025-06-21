using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;



namespace MyShop.DataAccess
{
    public class ApplicationDBContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        public ApplicationDBContext CreateDbContext(string[] args)
        {
            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json") // Must be at the root
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDBContext>();
            var connectionString = configuration.GetConnectionString("Defualt");

            builder.UseSqlServer(connectionString);

            return new ApplicationDBContext(builder.Options);
        }
    }
}
