using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Follow
    {
        public Guid FollowerId { get; set; }
        public ApplicationUser Follower { get; set; }

        public Guid FollowingId { get; set; }
        public ApplicationUser Following { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
