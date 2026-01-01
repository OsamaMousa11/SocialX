using SocialX.Core.Enumuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Conversation : BaseEntity
    {
        public ConversationType Type { get; set; }
        public string? Name { get; set; }
        public DateTime? LastMessageAt { get; set; }

        public ICollection<ConversationParticipant>? Participants { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }

}
