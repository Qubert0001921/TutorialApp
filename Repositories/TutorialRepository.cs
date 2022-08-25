using EmptyTest.Data;
using EmptyTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmptyTest.Repositories;
public interface ITutorialRepository : IBaseRepository<Tutorial, Guid>
{
    Task<Tutorial?> FindByNameAndAccountIdAsync(Guid accountId, string name);
    Task<IEnumerable<Tutorial>> FindAllWithValuesAsync();
    Task<IEnumerable<Tutorial>> FindPublicWithValuesAsync();
    Task<IEnumerable<Tutorial>> FindAllByAccountIdWithValues(Guid accountId);
    Task<Tutorial?> FindByIdWithValues(Guid id);
}

public class TutorialRepository : BaseRepository<Tutorial, Guid>, ITutorialRepository
{
    public TutorialRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Tutorial>> FindAllByAccountIdWithValues(Guid accountId)
    {
        return await _dbContext.Tutorials
            .Include(x => x.Account)
            .Include(x => x.Sections)
            .Where(x => x.AccountId == accountId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tutorial>> FindAllWithValuesAsync()
    {
        return await _dbContext.Tutorials
            .Include(x => x.Account)
            .Include(x => x.Sections)
            .ToListAsync();
    }

    public async Task<Tutorial?> FindByIdWithValues(Guid id)
    {
        return await _dbContext.Tutorials
            .Include(x => x.Account)
            .Include(x => x.Sections)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Tutorial?> FindByNameAndAccountIdAsync(Guid accountId, string name)
    {
        return await _dbContext.Tutorials
            .FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.AccountId == accountId);
    }

    public async Task<IEnumerable<Tutorial>> FindPublicWithValuesAsync()
    {
        return await _dbContext.Tutorials
            .Include(x => x.Account)
            .Include(x => x.Sections)
            .Where(x => x.IsPublic)
            .ToListAsync();
    }

}
