using Microsoft.EntityFrameworkCore;
using SocialX.Core.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.infrastraction.Configration
{
    public class UserConnectionConfig : IEntityTypeConfiguration<UserConnection>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserConnection> builder)
        {
            builder.HasKey(c => c.ConnectionId);

            builder.Property(c => c.ConnectionId)
                   .HasMaxLength(100);

            builder.HasOne(c => c.User)
                   .WithMany()
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.UserId);
        }
    }
}
