using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Bookmark
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid TweetId { get; set; }
        public Tweet Tweet { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
