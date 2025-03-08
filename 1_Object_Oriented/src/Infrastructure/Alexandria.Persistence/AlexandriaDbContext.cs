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

    //public DbSet<AuthorModelPublicationModel> AuthorModelPublicationModel { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);

    //    // Explicitly apply the configuration for AuthorPublicationModel
    //    modelBuilder.ApplyConfiguration(new AuthorsPublicationsConfiguration());
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
            .Entity<AuthorModel>()
            .HasMany(a => a.Publications)
            .WithMany(p => p.Authors)
            .UsingEntity<AuthorsPublications>("AuthorsPublications");
        //modelBuilder
        //    .Entity<AuthorModel>()
        //    .HasMany(a => a.Publications)
        //    .WithMany(p => p.Authors)
        //    .UsingEntity<AuthorModelPublicationModel>(
        //        "AuthorsPublications",
        //        l => l.HasOne<PublicationModel>().WithMany(),
        //        r => r.HasOne<AuthorModel>().WithMany(),
        //        j => j.HasKey("AuthorsId", "PublicationsId")
        //    );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        await OnPublicationModelCreated(cancellationToken);

        return base.SaveChanges();
    }

    private async Task OnPublicationModelCreated(CancellationToken cancellationToken)
    {
        var entries = ChangeTracker
            .Entries<PublicationModel>()
            .Where(e => e.State is EntityState.Added);

        foreach (var entry in entries)
        {
            var publication = entry.Entity;
            var missingAuthorIds = new List<long>();
            foreach (var authorId in publication.AuthorsIds)
            {
                var author = await FindAsync<AuthorModel>([authorId], cancellationToken);
                if (author is null)
                {
                    missingAuthorIds.Add(authorId);
                    continue;
                }

                // Join Author and Publication
                //var authorPublication = new AuthorModelPublicationModel
                //{
                //    AuthorId = authorId,
                //    PublicationId = publication.Id,
                //};

                //await AuthorModelPublicationModel.AddAsync(authorPublication, cancellationToken);
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
