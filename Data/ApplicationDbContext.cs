using EmptyTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmptyTest.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Tutorial> Tutorials { get; set; }
    public DbSet<Section> Sections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Topic>()
            .Property(x => x.DocumentFilePath)
            .IsRequired(false);

        modelBuilder.Entity<Topic>()
            .Property(x => x.VideFilePath)
            .IsRequired(false);
    }

    //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    //{
    //	foreach (var entry in this.ChangeTracker.Entries().Where(x => x.Entity is GuidEntity ))
    //	{
    //		var entity = entry.Entity as GuidEntity;
    //		if (entity is not null)
    //			entity.Id = Guid.NewGuid();
    //	}
    //	return base.SaveChangesAsync(cancellationToken);
    //}
}
