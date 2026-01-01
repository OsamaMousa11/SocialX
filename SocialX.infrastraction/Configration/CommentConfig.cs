using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Domain.Entites;

namespace SocialX.infrastraction.Configration
{
    public class CommentConfig : BaseEntityConfiguration<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            base.Configure(builder); 

        
            builder.Property(c => c.Content)
                   .IsRequired()
                   .HasMaxLength(280); 

          
            builder.HasOne(c => c.Tweet)
                   .WithMany(t => t.Comments)
                   .HasForeignKey(c => c.TweetId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.ParentComment)
                   .WithMany(pc => pc.Replies)
                   .HasForeignKey(c => c.ParentCommentId)
                    .OnDelete(DeleteBehavior.NoAction);


            builder.HasIndex(c => c.TweetId);
            builder.HasIndex(c => c.UserId);
            builder.HasIndex(c => c.ParentCommentId);
            builder.HasIndex(c => c.CreatedAt);

        
            builder.HasIndex(c => new { c.TweetId, c.CreatedAt });
            builder.HasIndex(c => new { c.ParentCommentId, c.CreatedAt });

            builder.ToTable("Comments");
        }
    }
}