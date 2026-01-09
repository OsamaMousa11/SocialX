using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.MentionDto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface IMentionService
    {
     
        Task<ApiResponse<PaginatedResult<MentionResponse>>> GetUserMentionsAsync(
            Guid userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

     
    
    
     
    }
}