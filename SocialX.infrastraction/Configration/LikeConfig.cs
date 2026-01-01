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
    internal class LikeConfig : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
           builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).HasDefaultValueSql("NEWID()");

            builder.Property(l => l.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");


            builder.HasOne(l => l.User)
                   .WithMany()
                   .HasForeignKey(l => l.UserId).OnDelete(DeleteBehavior.Cascade); ;

            builder.HasOne(l => l.Tweet)
                   .WithMany()
                   .HasForeignKey(l => l.TweetId)
                   .OnDelete(DeleteBehavior.NoAction) ;

            builder.HasOne(l => l.Comment)
                   .WithMany()
                   .HasForeignKey(l => l.CommentId)
                   .OnDelete(DeleteBehavior.Cascade).OnDelete(DeleteBehavior.NoAction) ;

            builder.ToTable("Likes");

        }
    }
}
