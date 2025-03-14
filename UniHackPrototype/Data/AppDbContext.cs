using Microsoft.EntityFrameworkCore;
using MyAspNetVueApp.Models;

namespace MyAspNetVueApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                var connectionString = "Data Source=app.db";
                options.UseSqlite(connectionString);
            }
        }
    }
}
