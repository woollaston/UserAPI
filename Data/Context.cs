using UserAPI.Models;
using Microsoft.EntityFrameworkCore;
using UserAPI.Interfaces;

namespace UserAPI.Data
{
    public class Context : DbContext, IDbContext
    {
        private string DbPath { get; }

        public Context()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "api.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(nameof(User.Id)).HasColumnType("uniqueidentifier");
                entity.Property(nameof(User.FirstName)).HasColumnType("nvarchar");
                entity.Property(nameof(User.LastName)).HasColumnType("nvarchar");
                entity.Property(nameof(User.Email)).HasColumnType("nvarchar");
                entity.Property(nameof(User.Address)).HasColumnType("nvarchar");
            });
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");

        public async Task Save<TEntity>(TEntity entity) where TEntity : class
        {
            await AddAsync(entity);
            await SaveChangesAsync();
        }
    }
}