using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.TweetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface IBookmarkService
    {
        Task BookmarkAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default);
        Task UnbookmarkAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default);
        Task<bool> IsBookmarkedAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default);

        Task<PaginatedResult<TweetResponse>> GetUserBookmarksAsync(
            Guid userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
    }

}

