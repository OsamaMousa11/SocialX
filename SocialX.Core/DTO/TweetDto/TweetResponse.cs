using SocialX.Core.DTO.AttachmentDto;
using SocialX.Core.DTO.MentionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.TweetDto
{
    public class TweetResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string? ProfileBackgroundUrl { get; set; }

   
        public int LikesCount { get; set; }
        public int RetweetsCount { get; set; }
        public int CommentsCount { get; set; }
        public int BookmarksCount { get; set; }

        public bool IsLikedByCurrentUser { get; set; }
        public bool IsRetweetedByCurrentUser { get; set; }
        public bool IsBookmarkedByCurrentUser { get; set; }

      
        public List<AttachmentResponse> Attachments { get; set; } = new();

        public List<string> Hashtags { get; set; } = new();

        public List<MentionResponse> Mentions { get; set; } = new();

  
        public TweetResponse? OriginalTweet { get; set; } // recursive لو quote
    }
}
