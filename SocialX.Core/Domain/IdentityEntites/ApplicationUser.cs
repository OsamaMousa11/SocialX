using Microsoft.AspNetCore.Identity;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.AuthenticationDTO;
using static System.Collections.Specialized.BitVector32;

namespace SocialX.Core.storeCore.Domain.IdentityEntites
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; }

   
        public Profile Profile { get; set; }

      
        public ICollection<Tweet>? Tweets { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Bookmark>? Bookmarks { get; set; }
    
        public ICollection<Message>? Messages { get; set; }

         public ICollection<Mention>? Mentions { get; set; }

      
        public ICollection<Notification>? ReceivedNotifications { get; set; }
        public ICollection<Notification>? SentNotifications { get; set; }

     
        public ICollection<Follow>? Followers { get; set; }
        public ICollection<Follow>? Following { get; set; }

       
        public ICollection<ConversationParticipant>? ConversationParticipants { get; set; }

        
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}