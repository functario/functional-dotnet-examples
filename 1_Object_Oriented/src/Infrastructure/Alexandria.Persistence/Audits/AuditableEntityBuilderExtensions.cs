using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Alexandria.Persistence.Audits;

internal static class AuditableEntityBuilderExtensions
{
    public static EntityTypeBuilder ConfigureAuditProperties(this EntityTypeBuilder builder)
    {
        builder.Property<DateTimeOffset>(IAuditable.CreatedDate);
        builder.Property<DateTimeOffset>(IAuditable.UpdatedDate);
        return builder;
    }

    public static OwnedNavigationBuilder ConfigureAuditProperties(
        this OwnedNavigationBuilder builder
    )
    {
        builder.Property<DateTimeOffset>(IAuditable.CreatedDate);
        builder.Property<DateTimeOffset>(IAuditable.UpdatedDate);
        return builder;
    }
}
