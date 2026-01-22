using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.MentionDto;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface IMentionService
    {

        Task<bool> CreateMentionsFromContentAsync(
                    Guid contentId,
                    string content,
                    bool isTweet = true,
                    CancellationToken cancellationToken = default);






    }
}