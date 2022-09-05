using EmptyTest.Data;
using EmptyTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmptyTest.Repositories;
public interface ISectionRepository : IBaseRepository<Section, Guid>
{
    Task<Section?> FindSectionByNameAndTutorialId(string sectionName, Guid tutorialId);
    Task<IEnumerable<Section>> FindSectionsByTutorialIdWithValuesAsync(Guid tutorialId);
}

public class SectionRepository : BaseRepository<Section, Guid>, ISectionRepository
{
    public SectionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Section?> FindSectionByNameAndTutorialId(string sectionName, Guid tutorialId)
    {
        return await _dbContext.Sections.FirstOrDefaultAsync(x => x.Name.ToLower() == sectionName.ToLower() && x.TutorialId == tutorialId);
    }

    public async Task<IEnumerable<Section>> FindSectionsByTutorialIdWithValuesAsync(Guid tutorialId)
    {
        return await _dbContext
            .Sections
            .Include(x => x.Topics)
            .Where(x => x.TutorialId == tutorialId)
            .ToListAsync();
    }
}
