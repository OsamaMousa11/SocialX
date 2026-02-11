using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Domain.Entites;

namespace SocialX.infrastraction.Configration
{
    public class AttachmentConfig : BaseEntityConfiguration<Attachment>
    {
        public override void Configure(EntityTypeBuilder<Attachment> builder)
        {
            base.Configure(builder);

            
            builder.Property(m => m.FileUrl)
                   .IsRequired()
                   .HasMaxLength(2083);

            builder.Property(m => m.Type)
                   .IsRequired()
                   .HasConversion<string>();

           
            builder.ToTable(t => t.HasCheckConstraint(
                "CK_Media_OneParent",
                "(TweetId IS NOT NULL AND CommentId IS NULL AND MessageId IS NULL) OR " +
                "(TweetId IS NULL AND CommentId IS NOT NULL AND MessageId IS NULL) OR " +
                "(TweetId IS NULL AND CommentId IS NULL AND MessageId IS NOT NULL)"
            ));

            
            builder.HasOne(m => m.Tweet)
                   .WithMany(t => t.Attachments)
                   .HasForeignKey(m => m.TweetId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Comment)
                   .WithMany(c => c.Attachments)
                   .HasForeignKey(m => m.CommentId)
                .OnDelete(DeleteBehavior.NoAction);

      


            builder.HasIndex(m => m.TweetId);
            builder.HasIndex(m => m.CommentId);
            builder.HasIndex(m => m.MessageId);
            builder.HasIndex(m => m.Type);

            builder.ToTable("Attachments");
        }
    }
}