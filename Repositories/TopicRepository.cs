using EmptyTest.Data;
using EmptyTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmptyTest.Repositories;
public interface ITopicRepository : IBaseRepository<Topic, Guid>
{
    Task<Topic?> FindByNameAsync(string name);
    Task<Topic?> FindByNameAndSectionIdAsync(string name, Guid sectionId);
}

public class TopicRepository : BaseRepository<Topic, Guid>, ITopicRepository
{
    public TopicRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Topic?> FindByNameAndSectionIdAsync(string name, Guid sectionId)
    {
        return await _dbContext.Topics.FirstOrDefaultAsync(x => x.Title.ToLower() == name.ToLower() && x.SectionId == sectionId);
    }

    public async Task<Topic?> FindByNameAsync(string name)
    {
        return await _dbContext.Topics.FirstOrDefaultAsync(x => x.Title.ToLower() == name.ToLower());
    }
}
