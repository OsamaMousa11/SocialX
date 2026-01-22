using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.CommentDto;
using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.TweetDto;
using SocialX.Core.Exceptions;
using SocialX.Core.IUnitofWork;
using SocialX.Core.ServiceContract;

namespace SocialX.Core.Service
{
    public class TweetService : ITweetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashtagService _hashtagService;
        private readonly IMentionService _mentionService;
        private readonly ILogger<TweetService> _logger;

        public TweetService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHashtagService hashtagService,
            IMentionService mentionService,
            ILogger<TweetService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashtagService = hashtagService;
            _mentionService = mentionService;
            _logger = logger;
        }


        public async Task<TweetResponse> AddTweetAsync(
         Guid userId,
         TweetAddRequest request,
         CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var tweet = _mapper.Map<Tweet>(request);
            tweet.UserId = userId;
            tweet.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Tweet>()
                .AddAsync(tweet, cancellationToken);

            if (request.Hashtags?.Any() == true)
            {
                foreach (var tag in request.Hashtags)
                {
                    await _hashtagService.CreateHashtagAsync(tag, cancellationToken);
                }
            }

            if (request.MentionedUserIds?.Any() == true)
            {
                await _mentionService.CreateMentionsFromContentAsync(
                    tweet.Id,
                    request.Content,
                    isTweet: true,
                    cancellationToken);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation(
                "User {UserId} created tweet {TweetId}",
                userId, tweet.Id);

            return _mapper.Map<TweetResponse>(tweet);
        }



        public async Task<TweetResponse> UpdateTweetAsync(
            Guid userId,
            Guid tweetId,
            TweetUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var tweet = await _unitOfWork.Repository<Tweet>()
                .FindAsync(
                    t => t.Id == tweetId && !t.IsDeleted,
                    includeProperties: "Attachments,Mentions",
                    cancellationToken: cancellationToken);

            if (tweet == null)
                throw new NotFoundException("Tweet not found");

            if (tweet.UserId != userId)
                throw new ForbiddenException("You are not allowed to update this tweet");

            tweet.Content = request.Content;
            tweet.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Tweet>().Update(tweet);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return await GetTweetByIdAsync(userId, tweetId, cancellationToken);
        }

     
        public async Task<bool> DeleteTweetAsync(
            Guid userId,
            Guid tweetId,
            CancellationToken cancellationToken = default)
        {
            var tweet = await _unitOfWork.Repository<Tweet>()
                .FindAsync(
                    t => t.Id == tweetId && !t.IsDeleted,
                    cancellationToken: cancellationToken);

            if (tweet == null)
                throw new NotFoundException("Tweet not found");

            if (tweet.UserId != userId)
                throw new ForbiddenException("You are not allowed to delete this tweet");

            tweet.IsDeleted = true;
          

            _unitOfWork.Repository<Tweet>().Update(tweet);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation(
                "User {UserId} deleted tweet {TweetId}",
                userId, tweetId);

            return true;
        }

 
        public async Task<TweetResponse> GetTweetByIdAsync(
            Guid? userId,
            Guid tweetId,
            CancellationToken cancellationToken = default)
        {
            var tweet = await _unitOfWork.Repository<Tweet>()
                .FindAsync(
                    t => t.Id == tweetId ,
                 
                    cancellationToken: cancellationToken);

            if (tweet == null)
                throw new NotFoundException("Tweet not found");

            return _mapper.Map<TweetResponse>(tweet);
        }

       
        public async Task<PaginatedResult<TweetResponse>> GetFeedAsync(
            Guid userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await _unitOfWork.Repository<Tweet>()
                .CountAsync(t => !t.IsDeleted, cancellationToken);

            var tweets = await _unitOfWork.Repository<Tweet>()
                .GetPagedAsync(
                    pageNumber,
                    pageSize,
                    predicate: t => !t.IsDeleted,
                    orderBy: q => q.OrderByDescending(t => t.CreatedAt),
                    includeProperties: "User.Profile,Attachments",
                    cancellationToken: cancellationToken);

            var responses = _mapper.Map<List<TweetResponse>>(tweets);

            return new PaginatedResult<TweetResponse>(
                responses,
                totalCount,
                pageNumber,
                pageSize);
        }
    }
}
