using Microsoft.AspNetCore.Identity;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.AuthenticationDTO;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SocialX.Core.storeCore.Domain.IdentityEntites
{
    public class ApplicationUser :IdentityUser<Guid>
    {

     
        public Profile Profile { get; set; }

        public ICollection<Tweet> Tweets { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}