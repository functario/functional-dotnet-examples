using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alexandria.Persistence.Configurations;

internal class BookAuthorsConfiguration : IEntityTypeConfiguration<BookAuthors>
{
    public void Configure(EntityTypeBuilder<BookAuthors> builder)
    {
        builder.ToTable("BookAuthors");
        builder.HasKey(ba => ba.Id);

        builder.HasOne(ba => ba.Book).WithMany(b => b.BookAuthors).HasForeignKey(ba => ba.BookId);

        builder
            .HasIndex(ba => ba.AuthorId) // Add index for fast lookups
            .HasDatabaseName("IX_BookAuthor_AuthorId");
    }
}
