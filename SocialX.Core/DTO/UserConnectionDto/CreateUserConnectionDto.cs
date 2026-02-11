using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.UserConnectionDto
{
    public class CreateUserConnectionDto
    {
        public Guid UserId { get; set; }
        public string ConnectionId { get; set; }
    }

}
