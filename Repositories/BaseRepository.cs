using Microsoft.EntityFrameworkCore;
using Write_from_csv_to_database.Entities;

namespace Write_from_csv_to_database.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbContext _context;
    public BaseRepository(DbContext context)
    {
        _context = context;
    }
    public virtual Task<T?> GetByIdAsync(int id)
    {
        return _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }
    public virtual Task<List<T>> GetListAsync()
    {
        return _context.Set<T>().ToListAsync();
    }
    public virtual Task CreateAsync(T entity)
    {
        entity.Id = 0;
        _context.Add(entity);
        return _context.SaveChangesAsync();
    }
    public virtual Task UpdateAsync(T entity)
    {
        _context.Update(entity);
        return _context.SaveChangesAsync();
    }
    public virtual async Task DeleteAsync(int id)
    {
        var entityToDelete = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        if (entityToDelete is null) return;
        _context.Set<T>().Remove(entityToDelete);
        await _context.SaveChangesAsync();
    }
}
