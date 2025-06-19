using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class BaseApiDbContext : DbContext, IBaseApiDbContext
    {
        public BaseApiDbContext(DbContextOptions<BaseApiDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }        

        public DbSet<T> Queryable<T>() where T : class
        {
            return Set<T>();
        }
    }
}
