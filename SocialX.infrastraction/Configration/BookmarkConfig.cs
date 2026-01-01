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
    public class BookmarkConfig :IEntityTypeConfiguration<Bookmark>
    {
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {  
           
            builder.HasKey(b => new { b.UserId, b.TweetId });

            builder.HasOne(b => b.User)
                   .WithMany(u => u.Bookmarks)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Tweet)
                   .WithMany(t => t.Bookmarks)
                   .HasForeignKey(b => b.TweetId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.Property(b => b.CreatedAt)
       .IsRequired()
       .HasDefaultValueSql("GETUTCDATE()");


            builder.HasIndex(b => b.TweetId);
            builder.HasIndex(b => b.UserId);
            builder.HasIndex(b => b.CreatedAt);

            builder.ToTable("Bookmarks");
        }
    }
}
