using ApplicationCore.Entities.Identity;
using ApplicationCore.Entities.Main;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    /// <summary>
    /// Основной контекст приложения
    /// </summary>
    public class MainContext : IdentityDbContext<User, Role, string>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=MyDatabase.db");
        }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}