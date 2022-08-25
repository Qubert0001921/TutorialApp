using EmptyTest.Data;
using Microsoft.EntityFrameworkCore;

namespace EmptyTest.Repositories;

public interface IBaseRepository<TData, TID> where TData : class where TID : struct
{
    Task<IEnumerable<TData>> FindAllAsync();
    Task<TData?> FindByIdAsync(TID id);
    Task RemoveAsync(TData entity);
    Task UpdateAsync(TData entity);
    Task CreateAsync(TData entity);
}

public class BaseRepository<TData, TID> : IBaseRepository<TData, TID> where TData : class where TID : struct
{
    protected readonly ApplicationDbContext _dbContext;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(TData entity)
    {
        _dbContext.Set<TData>().Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<TData>> FindAllAsync()
    {
        return await _dbContext.Set<TData>().ToListAsync();
    }

    public async Task<TData?> FindByIdAsync(TID id)
    {
        return await _dbContext.Set<TData>().FindAsync(id);
    }

    public async Task RemoveAsync(TData entity)
    {
        _dbContext.Set<TData>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TData entity)
    {
        _dbContext.Set<TData>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }
}
