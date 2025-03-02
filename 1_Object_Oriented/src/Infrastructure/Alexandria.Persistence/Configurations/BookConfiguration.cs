using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alexandria.Persistence.Configurations;

internal class BookConfiguration : IEntityTypeConfiguration<BookModel>
{
    public void Configure(EntityTypeBuilder<BookModel> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(x => x.Id);

        builder
            .HasMany<AuthorModel>()
            .WithMany()
            .UsingEntity<BookAuthor>(
                j => j.HasOne<AuthorModel>().WithMany().HasForeignKey("AuthorId"), // Relationship with AuthorModel by AuthorId
                j => j.HasOne<BookModel>().WithMany().HasForeignKey("BookId"), // Relationship with BookModel by BookId
                j => j.HasKey(nameof(BookAuthor.Id))
            );
    }
}
