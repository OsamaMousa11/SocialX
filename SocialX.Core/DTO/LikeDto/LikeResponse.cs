using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.LikeDto
{
    public class LikeResponse
    {
        public Guid UserId { get; set; }
        public Guid? TweetId { get; set; }
        public Guid? CommentId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
