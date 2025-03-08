namespace Alexandria.Persistence.Configurations;

//internal class AuthorsPublicationsConfiguration : IEntityTypeConfiguration<AuthorPublicationModel>
//{
//    public void Configure(EntityTypeBuilder<AuthorPublicationModel> builder)
//    {
//        builder.ToTable("AuthorsPublications");
//        builder.HasKey(x => new { x.AuthorId, x.PublicationId });
//        builder.HasOne<AuthorModel>().WithMany().HasForeignKey(x => x.AuthorId);

//        builder.HasOne<PublicationModel>().WithMany().HasForeignKey(x => x.PublicationId);
//    }
//}
