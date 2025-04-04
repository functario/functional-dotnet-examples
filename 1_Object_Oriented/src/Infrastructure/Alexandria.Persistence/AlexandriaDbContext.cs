﻿using Alexandria.Persistence.Modules.Authors.Configurations;
using Alexandria.Persistence.Modules.Authors.Models;
using Alexandria.Persistence.Modules.Books.Configurations;
using Alexandria.Persistence.Modules.Books.Models;
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

    public DbSet<BookAuthorsModel> BookAuthors { get; init; }

    public DbSet<PublicationModel> Publications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Need to explicitly apply configuration, otherwise the shadow properties are not added to tables.
        modelBuilder.ApplyConfiguration(new AuthorsConfiguration());
        modelBuilder.ApplyConfiguration(new BooksConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
