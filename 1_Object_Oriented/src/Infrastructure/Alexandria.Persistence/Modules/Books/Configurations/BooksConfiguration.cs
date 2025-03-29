using Alexandria.Persistence.Modules.Books.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alexandria.Persistence.Modules.Books.Configurations;

internal class BooksConfiguration : IEntityTypeConfiguration<BookModel>
{
    public void Configure(EntityTypeBuilder<BookModel> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(b => b.Id);
        builder.Property<DateTimeOffset>("CreatedDate");
        builder.Property<DateTimeOffset>("UpdatedDate");
        builder.OwnsOne(
            b => b.Publication,
            od =>
            {
                od.ToTable("Publications");
                od.HasKey(x => x.Id);
                od.WithOwner().HasForeignKey(od => od.Id);
                od.Property(x => x.PublicationDate).IsRequired();
                od.Property<DateTimeOffset>("CreatedDate");
                od.Property<DateTimeOffset>("UpdatedDate");
            }
        );
    }
}
