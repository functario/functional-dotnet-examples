using Alexandria.Domain.AuthorDomain;
using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Persistence;

internal class AlexandriaDbContext : DbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public AlexandriaDbContext(DbContextOptions<AlexandriaDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        : base(options) { }

    public DbSet<AuthorModel> Authors { get; init; }

    public DbSet<BookModel> Books { get; init; }

    public DbSet<PublicationModel> Publications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Join table between Authors and Books
        modelBuilder
            .Entity<AuthorModel>()
            .HasMany(a => a.Books)
            .WithMany(p => p.Authors)
            .UsingEntity<AuthorsBooks>(AuthorsBooks.TableName);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        await OnBookModelCreated(cancellationToken);

        return base.SaveChanges();
    }

    private async Task OnBookModelCreated(CancellationToken cancellationToken)
    {
        var entries = ChangeTracker.Entries<BookModel>().Where(e => e.State is EntityState.Added);

        foreach (var entry in entries)
        {
            var book = entry.Entity;
            var missingAuthorIds = new List<long>();
            foreach (var authorId in book.Publication.AuthorsIds)
            {
                var author = await FindAsync<AuthorModel>([authorId], cancellationToken);
                if (author is null)
                {
                    missingAuthorIds.Add(authorId);
                    continue;
                }

                // Join Author and Authors via AuthorsBooks join table.
                book.Authors.Add(author);
            }

            if (missingAuthorIds.Count > 0)
            {
                throw new InvalidOperationException(
                    $"The {nameof(Author)}s with following Ids do not exist: [{string.Join(';', missingAuthorIds)}]"
                );
            }
        }
    }
}
