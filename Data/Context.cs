using UserAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace UserAPI.Data
{
    public class Context : DbContext
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
            //base.OnModelCreating(modelBuilder);
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
    }
}