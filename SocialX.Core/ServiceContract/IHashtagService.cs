using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.HashtagDto;
using SocialX.Core.DTO.TweetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
  public interface IHashtagService
    {
        Task<HashtagResponse> CreateHashtagAsync(string tagName, CancellationToken cancellationToken = default);
        Task<HashtagResponse> GetHashtagByIdAsync(Guid hashtagId, CancellationToken cancellationToken = default);
        Task<HashtagResponse> GetHashtagByNameAsync(string tagName, CancellationToken cancellationToken = default);

        Task<PaginatedResult<HashtagResponse>> SearchHashtagsAsync(string searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task<bool> DeleteHashtagAsync(Guid hashtagId, CancellationToken cancellationToken = default);
    }
}
