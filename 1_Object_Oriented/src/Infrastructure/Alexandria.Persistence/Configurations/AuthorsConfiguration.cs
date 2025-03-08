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
        builder
            .HasMany(a => a.Publications)
            .WithMany(p => p.Authors)
            .UsingEntity<AuthorModelPublicationModel>(
                "AuthorsPublications",
                l => l.HasOne<PublicationModel>().WithMany(),
                r => r.HasOne<AuthorModel>().WithMany(),
                j => j.HasKey("AuthorsId", "PublicationsId")
            );
    }
}
