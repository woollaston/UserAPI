using Microsoft.EntityFrameworkCore;
using UserAPI.Models;

namespace UserAPI.Interfaces
{
    public interface IDbContext
    {
        DbSet<User> Users { get;  }

        Task Save<TEntity>(TEntity entity) where TEntity : class;
    }
}
