using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.UserConnectionDto
{
    public class UserConnectionDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
        public DateTime ConnectedAt { get; set; }
        public bool IsConnected { get; set; }
    }
}
