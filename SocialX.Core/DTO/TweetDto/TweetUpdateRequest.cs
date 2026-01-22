using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.TweetDto
{
    public class TweetUpdateRequest
    {
        [Required(ErrorMessage = "Content is required")]
        [StringLength(280, ErrorMessage = "Content length can't be more than 280 characters.")]
        public string Content { get; set; } = string.Empty;

       
        public List<IFormFile>? NewMediaFiles { get; set; }

        public List<Guid>? MediaIdsToRemove { get; set; }

        public List<Guid>? MentionedUserIds { get; set; }
    }
}
