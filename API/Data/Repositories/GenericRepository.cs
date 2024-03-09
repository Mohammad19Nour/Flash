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
}