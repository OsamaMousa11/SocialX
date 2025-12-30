using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SocialX.Core.Domain.Entites
{
    public class Tweet : BaseEntity
    {
        public string Content { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        
        public Guid? OriginalTweetId { get; set; }
        public Tweet OriginalTweet { get; set; }

        public ICollection<Tweet> ReTweets { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Media> Media { get; set; }
    }

}
