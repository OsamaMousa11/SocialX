using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Domain.Entites;

namespace SocialX.infrastraction.Configration
{
    public class TweetHashtagConfig : IEntityTypeConfiguration<TweetHashtag>
    {
        public void Configure(EntityTypeBuilder<TweetHashtag> builder)
        {
         
            builder.HasKey(th => new { th.TweetId, th.HashtagId });

         
            builder.Property(th => th.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

  
            builder.HasOne(th => th.Tweet)
                   .WithMany(t => t.TweetHashtags)
                   .HasForeignKey(th => th.TweetId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(th => th.Hashtag)
                   .WithMany(h => h.TweetHashtags)
                   .HasForeignKey(th => th.HashtagId)
                   .OnDelete(DeleteBehavior.Cascade);

           
            builder.HasIndex(th => th.TweetId);
            builder.HasIndex(th => th.HashtagId);

            builder.ToTable("TweetHashtags");
        }
    }
}