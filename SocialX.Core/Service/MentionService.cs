using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialX.Core.Domain.Entites;

using SocialX.Core.DTO.Common;
using SocialX.Core.Helper;
using SocialX.Core.IUnitofWork;

using SocialX.Core.ServiceContract;
using SocialX.Core.storeCore.Domain.IdentityEntites;

using System.Linq;

namespace SocialX.Application.Services
{
    public class MentionService : IMentionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MentionService> _logger;

        public MentionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MentionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<bool>> CreateMentionsFromContentAsync(
            Guid contentId,  
            string content,
            bool isTweet = true,  
            CancellationToken cancellationToken = default)
        {
            var usernames = MentionHelper.ExtractUsernames(content).Distinct().ToList(); 

            if (!usernames.Any())
                return ApiResponse<bool>.SuccessResponse(true);

            
            var users = await _unitOfWork.Repository<ApplicationUser>()
                .FindAllAsync(u => usernames.Contains(u.UserName!), cancellationToken: cancellationToken);

            if (!users.Any())
                return ApiResponse<bool>.SuccessResponse(true);

           
            var existingMentions = await _unitOfWork.Repository<Mention>()
                .FindAllAsync(m => m.TweetId == contentId && users.Select(u => u.Id).Contains(m.MentionedUserId),
                    cancellationToken: cancellationToken);

            var newMentions = users
                .Where(u => !existingMentions.Any(em => em.MentionedUserId == u.Id))
                .Select(user => new Mention
                {
                    TweetId = contentId,  
                    MentionedUserId = user.Id,
                    CreatedAt = DateTime.UtcNow
                })
                .ToList();

            if (newMentions.Any())
            {
                await _unitOfWork.Repository<Mention>().AddRangeAsync(newMentions, cancellationToken);
                await _unitOfWork.CompleteAsync(cancellationToken);

                _logger.LogInformation("Created {Count} new mentions for content {ContentId}", newMentions.Count, contentId);
            }

            return ApiResponse<bool>.SuccessResponse(true);
        }
    }
}