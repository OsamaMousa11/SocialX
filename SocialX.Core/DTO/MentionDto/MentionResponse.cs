using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.MentionDto
{

    public class MentionResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        
        public Guid AuthorId { get; set; }
        public string AuthorUserName { get; set; } = string.Empty;
        public string AuthorNickName { get; set; } = string.Empty;
        public string? AuthorProfileImageUrl { get; set; }

 
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int RetweetsCount { get; set; }

    
        public bool IsLikedByCurrentUser { get; set; }
        public bool IsBookmarkedByCurrentUser { get; set; }
    }
}
