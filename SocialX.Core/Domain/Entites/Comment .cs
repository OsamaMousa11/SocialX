using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }

        public Guid TweetId { get; set; }
        public Tweet Tweet { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

      
        public Guid? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; }

        public ICollection<Media> Media { get; set; }
    }

}
