namespace ProjectP.Interfaces;

public interface IGenericRepository<T> where T : class
{
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T?> GetByIdAsync(int id);
    IQueryable<T> GetQueryable();
}