using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialX.Core.Domain.Entites;

namespace SocialX.infrastraction.Configration
{
    public class MessageConfig : BaseEntityConfiguration<Message>
    {
        public override void Configure(EntityTypeBuilder<Message> builder)
        {
            base.Configure(builder);

      
            builder.Property(m => m.Content)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(m => m.IsRead)
                   .IsRequired()
                   .HasDefaultValue(false);

          
            builder.HasOne(m => m.Conversation)
                   .WithMany(c => c.Messages)
                   .HasForeignKey(m => m.ConversationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Sender)
                   .WithMany(u => u.Messages)
                   .HasForeignKey(m => m.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

          
            builder.HasIndex(m => m.ConversationId);
            builder.HasIndex(m => m.SenderId);
            builder.HasIndex(m => m.IsRead);
            builder.HasIndex(m => m.CreatedAt);

           
            builder.HasIndex(m => new { m.ConversationId, m.CreatedAt });

            builder.ToTable("Messages");
        }
    }
}