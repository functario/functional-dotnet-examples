using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alexandria.Persistence.Configurations;

internal class BooksConfiguration : IEntityTypeConfiguration<BookModel>
{
    public void Configure(EntityTypeBuilder<BookModel> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(b => b.Id);
        builder.OwnsOne<PublicationModel>(
            b => b.Publication,
            od =>
            {
                od.ToTable("Publications");
                od.HasKey(x => x.Id);
                od.WithOwner().HasForeignKey(od => od.Id);
                od.Property(x => x.PublicationDate).IsRequired();
                od.Property(x => x.CreatedDate).IsRequired();
                od.Property(x => x.UpdatedDate).IsRequired();
            }
        );
    }
}
