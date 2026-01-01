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
    public class ConversationParticipantConfig : IEntityTypeConfiguration<ConversationParticipant>
    {
        public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
        {
            builder.HasKey(cp => new { cp.ConversationId, cp.UserId });

            builder.Property(cp => cp.JoinedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(cp => cp.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.HasOne(cp => cp.Conversation)
                   .WithMany(c => c.Participants)
                   .HasForeignKey(cp => cp.ConversationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cp => cp.User)
                   .WithMany(u => u.ConversationParticipants)
                   .HasForeignKey(cp => cp.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(cp => cp.ConversationId);
            builder.HasIndex(cp => cp.UserId);
            builder.HasIndex(cp => cp.IsActive);
            builder.HasIndex(cp => new { cp.UserId, cp.IsActive }); // للـ active chats

            builder.ToTable("ConversationParticipants");
        }
    }
}
