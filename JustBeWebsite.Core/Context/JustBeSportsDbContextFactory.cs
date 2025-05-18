using JustBeSports.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace JustBeSports.Core
{
    public class JustBeSportsDbContextFactory : IDesignTimeDbContextFactory<JustBeSportsDbContext>
    {
        public JustBeSportsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json") 
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<JustBeSportsDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new JustBeSportsDbContext(optionsBuilder.Options);
        }
    }
}
