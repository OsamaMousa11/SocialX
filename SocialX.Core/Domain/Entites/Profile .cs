using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Profile : BaseEntity
    {
        public string NickName { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? ProfileBackgroundImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

}
