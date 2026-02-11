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
        [Required(ErrorMessage = " name is required")]
        public string NickName { get; set; }


     

        public string? Bio { get; set; }

        [Required(ErrorMessage = "Profile image is required")]
        public IFormFile? ProfileImg { get; set; }

        [Required(ErrorMessage = "Profile background is required")]
        public IFormFile? ProfileBackground { get; set; }

        public GenderOptions Gender { get; set; } = GenderOptions.MALE;


    }
}
