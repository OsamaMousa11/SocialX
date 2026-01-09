using AutoMapper;
using Microsoft.Extensions.Logging;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.Common;
using SocialX.Core.DTO.LikeDto;
using SocialX.Core.IUnitofWork;
using SocialX.Core.ServiceContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialX.Core.Service
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<LikeService> _logger;

        public LikeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LikeService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public  async Task<ApiResponse<PaginatedResult<LikedUserResponse>>> GetCommentLikesAsync(Guid commentId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            //find comment
            var tweetExist = await _unitOfWork.Repository<Comment>().ExistsAsync(t => t.Id == commentId && !t.IsDeleted, cancellationToken);
            if (!tweetExist)
                return ApiResponse<PaginatedResult<LikedUserResponse>>.FailureResponse("comment not found");
            var totalCount = await _unitOfWork.Repository<Like>()
                    .CountAsync(l => l.CommentId == commentId, cancellationToken);

            var likes = await _unitOfWork.Repository<Like>()
                .GetPagedAsync(
                    pageNumber,
                    pageSize,
                    predicate: l => l.CommentId == commentId,
                    orderBy: q => q.OrderByDescending(l => l.CreatedAt),
                    includeProperties: "User.Profile",
                    cancellationToken: cancellationToken);

            var likedUsers = _mapper.Map<List<LikedUserResponse>>(likes);

            var paginatedResult = new PaginatedResult<LikedUserResponse>(likedUsers, totalCount, pageNumber, pageSize);

            return ApiResponse<PaginatedResult<LikedUserResponse>>.SuccessResponse(paginatedResult);
        }
        public  async Task<ApiResponse<PaginatedResult<LikedUserResponse>>> GetTweetLikesAsync(Guid tweetId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var tweetExists = await _unitOfWork.Repository<Tweet>().ExistsAsync(t => t.Id == tweetId && !t.IsDeleted, cancellationToken);

            if (!tweetExists)
                return ApiResponse<PaginatedResult<LikedUserResponse>>
       .FailureResponse("Tweet not found");
            var totalCount = await _unitOfWork.Repository<Like>()
                 .CountAsync(l => l.TweetId == tweetId, cancellationToken);

            var likes = await _unitOfWork.Repository<Like>()
                .GetPagedAsync(
                    pageNumber,
                    pageSize,
                    predicate: l => l.TweetId == tweetId,
                    orderBy: q => q.OrderByDescending(l => l.CreatedAt),
                    includeProperties: "User.Profile",
                    cancellationToken: cancellationToken);
            var likedUsers = _mapper.Map<List<LikedUserResponse>>(likes);

            var paginatedResult = new PaginatedResult<LikedUserResponse>(likedUsers, totalCount, pageNumber, pageSize);

            return ApiResponse<PaginatedResult<LikedUserResponse>>.SuccessResponse(paginatedResult);
        }

        public async Task<ApiResponse<int>> GetTweetLikesCountAsync(Guid tweetId,CancellationToken cancellationToken = default)
        {
            var count = await _unitOfWork.Repository<Like>()
              .CountAsync(l => l.TweetId == tweetId, cancellationToken);

            return ApiResponse<int>.SuccessResponse(count);
        }

        public async Task<ApiResponse<int>> GetCommentLikesCountAsync(
               Guid commentId,
               CancellationToken cancellationToken = default)
        {
            var count = await _unitOfWork.Repository<Like>()
                   .CountAsync(l => l.CommentId == commentId, cancellationToken);

            return ApiResponse<int>.SuccessResponse(count);
        }
        public async Task<bool> IsLikedTweetAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Like>()
                .ExistsAsync(l => l.UserId == userId && l.TweetId == tweetId, cancellationToken);
        }

        public async Task<bool> IsLikedCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Repository<Like>()
                .ExistsAsync(l => l.UserId == userId && l.CommentId == commentId, cancellationToken);
        }

        public  async Task<ApiResponse<bool>> LikeCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken = default)
        {
            //find comment
            var commentExists =  await _unitOfWork.Repository<Comment>().ExistsAsync(t => t.Id == commentId && !t.IsDeleted, cancellationToken);
            if(!commentExists)
                return ApiResponse<bool>.FailureResponse("comment not found");

            //  cheack if alreadyLiked 

            var alreadyLiked = await _unitOfWork.Repository<Like>().ExistsAsync(t => t.UserId == userId && t.CommentId == commentId, cancellationToken);
         
                if (alreadyLiked)
                    return ApiResponse<bool>.FailureResponse("Already liked");
            var like = new Like
            {
                UserId = userId,
                CommentId = commentId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Like>().AddAsync(like, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true, "Comment  liked successfully");

        }

        public async Task<ApiResponse<bool>> LikeTweetAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default)
        {
                 //find tweet 
            var tweetExists= await _unitOfWork.Repository<Tweet>().ExistsAsync(t=>t.Id == tweetId && !t.IsDeleted,cancellationToken);
            
            if (!tweetExists)
                return ApiResponse<bool>.FailureResponse("Tweet not found");

            //  cheack if alreadyLiked 

            var alreadyLiked=await _unitOfWork.Repository<Like>().ExistsAsync(t=>t.UserId== userId && t.TweetId == tweetId, cancellationToken);

            if(alreadyLiked)
                return ApiResponse<bool>.FailureResponse("Already liked");

            var like = new Like
            {
                UserId = userId,
                TweetId = tweetId,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Repository<Like>().AddAsync(like, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("User {UserId} liked tweet {TweetId}", userId, tweetId);

            return ApiResponse<bool>.SuccessResponse(true, "Tweet liked successfully"); 
        }

        public async Task<ApiResponse<bool>> UnlikeCommentAsync(Guid userId, Guid commentId, CancellationToken cancellationToken = default)
        {


            var like = await _unitOfWork.Repository<Like>().FindAsync(l => l.UserId == userId && l.CommentId == commentId, cancellationToken: cancellationToken);

            if (like == null)
                return ApiResponse<bool>.FailureResponse("Like not found");
            _unitOfWork.Repository<Like>().Delete(like);

            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("User {UserId} unliked comment {CommentId}", userId, commentId);

            return ApiResponse<bool>.SuccessResponse(true, "Comment unliked successfully");

        }

        public  async Task<ApiResponse<bool>> UnlikeTweetAsync(Guid userId, Guid tweetId, CancellationToken cancellationToken = default)
        {
            var like = await _unitOfWork.Repository<Like>().FindAsync(l => l.UserId == userId && l.TweetId == tweetId, cancellationToken: cancellationToken);

            if (like == null)
                return ApiResponse<bool>.FailureResponse("Like not found");
            _unitOfWork.Repository<Like>().Delete(like);

            await _unitOfWork.CompleteAsync(cancellationToken);

            _logger.LogInformation("User {UserId} unliked Tweet {tweetId}", userId, tweetId);

            return ApiResponse<bool>.SuccessResponse(true, "Tweet unliked successfully");
        }

      
    }
}
