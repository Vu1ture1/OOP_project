using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp.DataBaseFolder
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleRequest> Requests { get; set; }
    }
}
