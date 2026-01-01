using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class UserConnection
    {
        
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string ConnectionId { get; set; }
        public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
        public bool IsConnected { get; set; } = true;
    }

}
