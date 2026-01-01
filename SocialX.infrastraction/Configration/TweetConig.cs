using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.infrastraction.Configration
{
    public class TweetConig : BaseEntityConfiguration<Tweet>
    {
        public void Config(EntityTypeBuilder<Tweet> builder)
        {
            base.Configure(builder);

            builder.Property(t => t.Content)
                   .IsRequired()
                   .HasMaxLength(280);

            builder.HasOne(t => t.User)
               .WithMany(u => u.Tweets)
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.OriginalTweet)
                 .WithMany(t => t.ReTweets)
                 .HasForeignKey(t => t.OriginalTweetId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(t => t.UserId);
            builder.HasIndex(t => t.CreatedAt);
            builder.HasIndex(t => t.OriginalTweetId);

        }
    }
}
