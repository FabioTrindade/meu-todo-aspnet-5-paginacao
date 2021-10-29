using Microsoft.EntityFrameworkCore;
using Paginacao.Models;
using Paginacao.ViewModels;

namespace Paginacao.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }

        public virtual DbSet<TodoViewModel> TodoViewModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite(connectionString: "DataSource=app.db;Cache=Shared");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoViewModel>(e =>
            {
                e.HasNoKey();
            });
        }
    }
}
