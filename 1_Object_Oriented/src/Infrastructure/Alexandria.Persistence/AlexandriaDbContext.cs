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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        await Toto(cancellationToken);

        return base.SaveChanges();
    }

    private async Task Toto(CancellationToken cancellationToken)
    {
        var entries = ChangeTracker
    .Entries<PublicationModel>()
    .Where(e => e.State == EntityState.Added);

        foreach (var entry in entries)
        {
            var publication = entry.Entity;

            if (publication.AuthorsIds == null || publication.AuthorsIds.Count == 0)
            {
                throw new InvalidOperationException("A publication must have at least one author.");
            }

            var allExist = publication.AuthorsIds.Select(x =>
                FindAsync<AuthorModel>([x], cancellationToken)
            );

            var missingAuthorIds = new List<long>();
            foreach (var authorId in publication.AuthorsIds)
            {
                var author = await FindAsync<AuthorModel>([authorId], cancellationToken);
                if (author is null)
                {
                    missingAuthorIds.Add(authorId);
                }
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
