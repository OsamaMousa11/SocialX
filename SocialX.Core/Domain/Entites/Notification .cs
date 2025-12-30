using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }          // Receiver
        public ApplicationUser User { get; set; }

        public Guid ActorUserId { get; set; }     // Actor
        public ApplicationUser ActorUser { get; set; }

        public string Type { get; set; }           // Follow, Like, Comment, ReTweet
        public Guid EntityId { get; set; }         // TweetId / CommentId
        public bool IsRead { get; set; } = false;
    }

}
