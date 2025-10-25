using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Models;

namespace SmartRecyclingApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options)
        {

        }

        public DbSet<UtilizadorModel> Utilizadores { get; set; }

    }
}
