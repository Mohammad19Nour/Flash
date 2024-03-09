using Microsoft.EntityFrameworkCore.Storage;

namespace ProjectP.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<bool> SaveChanges();
    IDbContextTransaction BeginTransaction();
    bool HasChanges();
}