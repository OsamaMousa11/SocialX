using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Like
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid? TweetId { get; set; }
        public Tweet Tweet { get; set; }

        public Guid? CommentId { get; set; }
        public Comment Comment { get; set; }

        public DateTime CreatedAt { get; set; } 
   

    }

}
