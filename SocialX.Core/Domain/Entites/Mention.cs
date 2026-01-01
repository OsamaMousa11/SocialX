using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Mention
    {
        public Guid TweetId { get; set; }
        public Tweet Tweet { get; set; }

        public Guid MentionedUserId { get; set; }
        public ApplicationUser MentionedUser { get; set; } 
    }
}
