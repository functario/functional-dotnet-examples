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
                od.WithOwner().HasForeignKey(od => od.Id);
                od.Property(x => x.PublicationDate).IsRequired();
                od.Property(x => x.CreatedDate).IsRequired();
                od.Property(x => x.UpdatedDate).IsRequired();
                od.Property(x => x.AuthorsIds).IsRequired();
                //od.Property(x => x.AuthorsIds)
                //    .HasConversion(
                //        v => string.Join(",", v), // Convert list of long to comma-separated string
                //        v => v.Split(",", StringSplitOptions.None).Select(long.Parse).ToList() // Convert back to list of long
                //    );
            }
        );
    }
}
