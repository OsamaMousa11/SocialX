using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.DTO.BookMarkDto
{
    public class BookmarkResponse
    {
        public Guid TweetId { get; set; }
        public bool IsBookmarked { get; set; }
        public DateTime? BookmarkedAt { get; set; }

    }
}
