using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Domain.Entites;

namespace SocialX.infrastraction.Configration
{
    public class NotificationConfig : BaseEntityConfiguration<Notification>
    {
        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            
            builder.Property(n => n.Type)
                   .IsRequired()
                   .HasConversion<string>(); 

            builder.Property(n => n.Content)
                   .HasMaxLength(500);

            builder.Property(n => n.IsRead)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.HasOne(n => n.ReceiverUser)
                   .WithMany(u => u.ReceivedNotifications)
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(n => n.SenderUser)
                   .WithMany(u => u.SentNotifications)
                   .HasForeignKey(n => n.ActorUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasIndex(n => n.UserId);
            builder.HasIndex(n => n.ActorUserId);
            builder.HasIndex(n => n.IsRead);
            builder.HasIndex(n => n.CreatedAt);

            builder.HasIndex(n => new { n.UserId, n.IsRead, n.CreatedAt });

            builder.ToTable("Notifications");
        }
    }
}