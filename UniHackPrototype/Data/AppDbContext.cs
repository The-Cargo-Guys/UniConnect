using Microsoft.EntityFrameworkCore;
using MyAspNetVueApp.Models;

namespace MyAspNetVueApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = "Data Source=app.db"; 
            options.UseSqlite(connectionString);
        }
    }
}
