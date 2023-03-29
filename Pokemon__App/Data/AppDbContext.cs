using Microsoft.EntityFrameworkCore;
using Pokemon__App.Models; 
namespace Pokemon__App.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options)
        {
        }
        public DbSet<Pokemon> Pokemon { get; set; }
    }
}
