using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Models;

namespace SmartRecyclingApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<UtilizadorModel> Utilizadores { get; set; }
        public DbSet<ReciclagemModel> Reciclagem { get; set; }
        public DbSet<PedidoModel> Pedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UtilizadorModel>()
                .HasIndex(u => u.email)
                .IsUnique();
        }
    }
}
