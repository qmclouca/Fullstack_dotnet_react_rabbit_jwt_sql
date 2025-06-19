using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class BaseApiDbContextFactory : IDesignTimeDbContextFactory<BaseApiDbContext>
    {
        public BaseApiDbContext CreateDbContext(string[] args)
        {
            var projectPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../BaseApiAPI/BaseApiAPI"));
            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<BaseApiDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new BaseApiDbContext(builder.Options);
        }
    }
}
