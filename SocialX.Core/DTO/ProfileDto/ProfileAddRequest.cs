using Microsoft.AspNetCore.Http;
using SocialX.Core.Enumuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SocialX.Core.DTO.ProfileDto
{
    public class ProfileAddRequest
    {
        [Required]
        [MaxLength(50)]
        public string NickName { get; set; }

        [MaxLength(160)]
        public string? Bio { get; set; }

        public IFormFile? ProfileImage { get; set; }
        public IFormFile? BackgroundImage { get; set; }
    }
}
