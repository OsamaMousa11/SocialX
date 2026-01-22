using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.ProfileDto
{
    public class ProfileUpdateRequest
    {
        [Required]
        [MaxLength(50)]
        public string NickName { get; set; }

        [MaxLength(160)]
        public string? Bio { get; set; }

        public IFormFile? ProfileImageUrl { get; set; }
        public IFormFile? BackgroundImage { get; set; }
    }
}
