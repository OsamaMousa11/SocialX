using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Conversation : BaseEntity
    {
        public string Type { get; set; } // DM / Group

        public ICollection<ConversationParticipant> Participants { get; set; }
        public ICollection<Message> Messages { get; set; }
    }

}
