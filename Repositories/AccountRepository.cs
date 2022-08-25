using EmptyTest.Data;
using EmptyTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmptyTest.Repositories;

public interface IAccountRepository : IBaseRepository<Account, Guid>
{
    Task<Account?> FindByEmailAsync(string email);
}

public class AccountRepository : BaseRepository<Account, Guid>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Account?> FindByEmailAsync(string email)
    {
        return await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    }
}
