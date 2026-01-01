using SocialX.Core.Enumuration;
using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Entites
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }      
        public ApplicationUser ReceiverUser { get; set; }

        public Guid ActorUserId { get; set; }     
        public ApplicationUser SenderUser { get; set; }

       
        public NotificationType Type { get; set; }

        public Guid? EntityId { get; set; }

        public string? Content { get; set; }
        public bool IsRead { get; set; } = false;
    }

}
