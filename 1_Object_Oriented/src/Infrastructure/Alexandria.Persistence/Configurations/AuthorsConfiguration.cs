using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alexandria.Persistence.Configurations;

internal class AuthorsConfiguration : IEntityTypeConfiguration<AuthorModel>
{
    public void Configure(EntityTypeBuilder<AuthorModel> builder)
    {
        builder.ToTable("Authors");
        builder.HasKey(x => x.Id);
    }
}
