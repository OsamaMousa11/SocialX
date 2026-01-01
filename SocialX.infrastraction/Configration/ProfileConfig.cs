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
    public class ProfileConfig : BaseEntityConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            base.Configure(builder);

           
            builder.Property(p => p.NickName)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(p => p.Bio)
                   .HasMaxLength(250);

            builder.Property(p => p.ProfileImageUrl)
                   .HasMaxLength(2083);

            builder.Property(p => p.ProfileBackgroundImageUrl)
                   .HasMaxLength(2083);

            builder.HasOne(p => p.User)
                   .WithOne(u => u.Profile)
                   .HasForeignKey<Profile>(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.UserId)
                   .IsUnique();

            builder.ToTable("Profiles");

        }
    }

}
