using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Media : BaseEntity
    {
        public string FileUrl { get; set; }

        public Guid? TweetId { get; set; }
        public Tweet Tweet { get; set; }

        public Guid? CommentId { get; set; }
        public Comment Comment { get; set; }
    }

}
