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
    public class FollowConfig : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {

            builder.HasKey(f => new { f.FollowerId, f.FollowingId });

            builder.Property(f => f.CreatedAt)
                   .IsRequired().HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(f => f.Follower)
                   .WithMany(u => u.Following).HasForeignKey(f => f.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Following).WithMany(u => u.Followers).HasForeignKey(f => f.FollowingId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Follows");
        }
    }
}
