using Animou.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animou.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnType("nvarchar(50)");

            builder.Property(x => x.Image)
                .HasColumnType("varchar(100)");

            builder.OwnsOne(x => x.Email, tf =>
            {
                tf.Property(c => c.Address)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasColumnType("nvarchar(100)");
            });

            builder.HasMany(x => x.Medias)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.Comments)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.ToTable("Users");
        }
    }

    public class MediaMapping : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Medias");
        }
    }
}
