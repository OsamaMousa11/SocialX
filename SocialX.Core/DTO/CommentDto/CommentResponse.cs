using SocialX.Core.DTO.AttachmentDto;
using SocialX.Core.DTO.MentionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.CommentDto
{
   
        public class CommentResponse
        {
            public Guid Id { get; set; }
            public string Content { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public Guid TweetId { get; set; }     
            public Guid? ParentCommentId { get; set; }  
            public Guid UserId { get; set; }
            public string UserName { get; set; } = string.Empty;
            public string NickName { get; set; } = string.Empty;
            public string? ProfileImageUrl { get; set; }

            public int LikesCount { get; set; }
            public int RepliesCount { get; set; }

            public bool IsLikedByCurrentUser { get; set; }

          
            public List<AttachmentResponse> Attachments { get; set; } = new();
            public List<MentionResponse> Mentions { get; set; } = new();
        }
    }


