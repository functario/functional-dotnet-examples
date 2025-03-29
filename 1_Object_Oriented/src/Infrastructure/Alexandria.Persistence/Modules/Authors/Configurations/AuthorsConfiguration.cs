using Alexandria.Persistence.Audits;
using Alexandria.Persistence.Modules.Authors.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alexandria.Persistence.Modules.Authors.Configurations;

internal class AuthorsConfiguration : IEntityTypeConfiguration<AuthorModel>
{
    public void Configure(EntityTypeBuilder<AuthorModel> builder)
    {
        builder.ToTable("Authors");
        builder.HasKey(x => x.Id);
        builder.ConfigureAuditProperties();
    }
}
