using Microsoft.EntityFrameworkCore;
using ProjectP.Entities;
using ProjectP.Interfaces;

namespace ProjectP.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DataContext _context;

    public GenericRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public IQueryable<T> GetQueryable()
    {
        return _context.Set<T>().AsQueryable();
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.Entry(entity).State = EntityState.Added;
    }

    public void Update(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}