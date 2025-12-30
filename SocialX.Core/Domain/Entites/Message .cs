using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Message : BaseEntity
    {
        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; }

        public Guid SenderId { get; set; }
        public User Sender { get; set; }

        public string Content { get; set; }
    }

}
