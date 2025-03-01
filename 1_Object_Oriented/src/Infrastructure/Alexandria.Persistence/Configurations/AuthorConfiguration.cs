using Alexandria.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alexandria.Persistence.Configurations;

internal class AuthorConfiguration : IEntityTypeConfiguration<AuthorModel>
{
    public void Configure(EntityTypeBuilder<AuthorModel> builder)
    {
        // dotnet ef migrations add CreateAuthorTable -- "Server=127.0.0.1,50925;User ID=sa;Password=Password1234!;TrustServerCertificate=true;Database=alexandria" --verbose
        builder.ToTable("Author");
        builder.HasKey(x => x.Id);
    }
}
