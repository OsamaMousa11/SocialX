using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class TweetHashtag
    {
        public Guid TweetId { get; set; }
        public Tweet Tweet { get; set; }

        public Guid HashtagId { get; set; }
        public Hashtag Hashtag { get; set; }
    }

}
