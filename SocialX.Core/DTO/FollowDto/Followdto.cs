using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.FollowDto
{
    public class CreateFollowDto
    {
        public Guid FollowingId { get; set; }
    }

    public class FollowDto
    {
        public Guid FollowerId { get; set; }
        public string FollowerName { get; set; }
        public string FollowerEmail { get; set; }
        public Guid FollowingId { get; set; }
        public string FollowingName { get; set; }
        public string FollowingEmail { get; set; }
        public DateTime CreatedAt { get; set; }
    }


    public class FollowerDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime FollowedAt { get; set; }
    }


  
    public class FollowingDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime FollowedAt { get; set; }
    }


}
