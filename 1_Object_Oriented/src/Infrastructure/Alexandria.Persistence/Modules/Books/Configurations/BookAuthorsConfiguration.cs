using Alexandria.Persistence.Modules.Books.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alexandria.Persistence.Modules.Books.Configurations;

internal class BookAuthorsConfiguration : IEntityTypeConfiguration<BookAuthorsModel>
{
    public void Configure(EntityTypeBuilder<BookAuthorsModel> builder)
    {
        // csharpier-ignore-start
        builder.ToTable("BookAuthors");
        builder.HasKey(ba => ba.Id);

        builder.HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId);

        builder
            .HasIndex(ba => ba.AuthorId)
            .HasDatabaseName("IX_BookAuthors_AuthorId");

        // csharpier-ignore-ending
    }
}
