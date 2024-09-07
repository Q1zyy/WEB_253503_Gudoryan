using Microsoft.EntityFrameworkCore;
using WEB_253503_Gudoryan.Domain.Entities;

namespace WEB_253503_Gudoryan.API.Data
{
    public class AppDbContext : DbContext
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Game> Games { get; set; }

        public DbSet<Category> Categories { get; set; }


    }
}
