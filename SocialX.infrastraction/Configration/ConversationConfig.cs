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
    public class ConversationConfig : BaseEntityConfiguration<Conversation>
    {
        public override void Configure(EntityTypeBuilder<Conversation> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Type)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(c => c.Name)
                   .HasMaxLength(100);

            builder.Property(c => c.LastMessageAt);

            // Indexes
            builder.HasIndex(c => c.CreatedAt);
            builder.HasIndex(c => c.LastMessageAt);

            builder.ToTable("Conversations");
        }
    }
}
