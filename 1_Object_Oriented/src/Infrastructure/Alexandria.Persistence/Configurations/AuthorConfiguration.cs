using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alexandria.Persistence.Configurations;

internal class AuthorConfiguration : IEntityTypeConfiguration<AuthorModel>
{
    public void Configure(EntityTypeBuilder<AuthorModel> builder)
    {
        builder.ToTable("Authors");
        builder.HasKey(x => x.Id);

        //builder
        //    .HasMany<BookModel>()
        //    .WithMany()
        //    .UsingEntity<BookAuthor>(
        //        j => j.HasOne<BookModel>().WithMany().HasForeignKey("BookId"), // Relationship with BookModel by BookId
        //        j => j.HasOne<AuthorModel>().WithMany().HasForeignKey("AuthorId") // Relationship with AuthorModel by AuthorId
        //    );
    }
}
