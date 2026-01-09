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
    public class MentionConfiguration : IEntityTypeConfiguration<Mention>
    {
        public void Configure(EntityTypeBuilder<Mention> builder)
        {
            
            builder.HasKey(m => new { m.TweetId, m.MentionedUserId });

            builder.Property(e => e.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(m => m.Tweet)
               .WithMany(t => t.Mentions)
               .HasForeignKey(m => m.TweetId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.MentionedUser)
                   .WithMany()
                   .HasForeignKey(m => m.MentionedUserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(m => m.CreatedAt);
            builder.ToTable("Mentions");
        }
    }

}
