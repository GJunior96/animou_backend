using Animou.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Animou.Data.Mappings
{
    public class CommentMapping : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Text)
                .HasColumnType("nvarchar(1000)");

            builder.HasMany(x => x.Likes)
                .WithOne(x => x.Comment)
                .HasForeignKey(x => x.CommentId);

            builder.HasMany(x => x.Dislikes)
                .WithOne(x => x.Comment)
                .HasForeignKey(x => x.CommentId);

            //builder.HasOne(x => x.Parent)
            //    .WithMany(x => x.Replies)
            //    .HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(x => x.Replies)
                .WithOne(x => x.Parent)
                .HasForeignKey(x => x.ParentId);

            //builder.HasOne(x => x.ParentUser)
            //    .WithMany(x => x.Comments)
            //    .HasForeignKey(x => x.ParentUserId).OnDelete(DeleteBehavior.ClientSetNull);

            builder.ToTable("Comments");
        }
    }

    public class LikeMapping : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Likes");
        }
    }

    public class DislikeMapping : IEntityTypeConfiguration<Dislike>
    {
        public void Configure(EntityTypeBuilder<Dislike> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Dislikes");
        }
    }
}
