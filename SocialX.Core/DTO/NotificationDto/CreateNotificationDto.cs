using SocialX.Core.Enumuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.NotificationDto
{
    public class CreateNotificationDto
    {
        public Guid UserId { get; set; }
        public Guid ActorUserId { get; set; }
        public NotificationType Type { get; set; }
        public Guid? EntityId { get; set; }
        public string? Content { get; set; }
    }
}
