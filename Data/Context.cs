using UserAPI.Models;
using Microsoft.EntityFrameworkCore;
using UserAPI.Interfaces;
using Microsoft.Extensions.Options;

namespace UserAPI.Data
{
    public class Context : DbContext, IDbContext
    {
        private string DbPath { get; }

        public Context(IOptions<Config> config)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, config.Value.DbFileName);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(nameof(User.Id)).HasColumnType("uniqueidentifier");
                entity.Property(nameof(User.FirstName)).HasColumnType("nvarchar");
                entity.Property(nameof(User.LastName)).HasColumnType("nvarchar");
                entity.Property(nameof(User.DateOfBirth)).HasColumnType("nvarchar");
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