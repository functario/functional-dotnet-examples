using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alexandria.Persistence.Configurations;

internal class BookConfiguration : IEntityTypeConfiguration<BookModel>
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
            }
        );

        //builder.Property<PublicationModel>(b =>
        //{
        //        Id = b.Publication!.Id,
        //        BookId = b.Id,
        //        CreatedDate = b.CreatedDate,
        //        UpdatedDate = b.UpdatedDate,
        //        PublicationDate = b.PublicationDate
        //    })
    }
}
