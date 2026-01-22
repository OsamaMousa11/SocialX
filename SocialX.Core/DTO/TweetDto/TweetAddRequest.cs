using Microsoft.AspNetCore.Http;
using SocialX.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.TweetDto
{
    public class TweetAddRequest
    {
        [Required(ErrorMessage = "Content is required")]

        [StringLength(280, ErrorMessage = "Content length can't be more than 280 characters.")]
        public string Content { get; set; }

        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mov" })]
        public List<IFormFile>? AttachmentFiles { get; set; }

        public List<string>? Hashtags { get; set; }
        public Guid? OriginalTweetId { get; set; }

        public List<Guid>? MentionedUserIds { get; set; }

       }


    }

