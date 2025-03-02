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
    }
}
