//using Alexandria.Persistence.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Alexandria.Persistence.Configurations;

//internal class PublicationConfiguration : IEntityTypeConfiguration<PublicationModel>
//{
//    public void Configure(EntityTypeBuilder<PublicationModel> builder)
//    {
//        // csharpier-ignore-start
//        builder.ToTable("Publications");
//        builder.HasKey(x => x.Id);

//        //builder
//        //    .HasOne<BookModel>()
//        //    .WithOne()
//        //    .HasForeignKey<BookModel>();

//        builder
//            .HasMany<AuthorModel>()
//            .WithOne()
//            .HasForeignKey(author => author.Id)
//            .IsRequired();

//        // csharpier-ignore-end
//    }
//}
