using AutoMapper;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.FollowDto;
using SocialX.Core.DTO.NotificationDto;
using SocialX.Core.Enumuration;
using SocialX.Core.IUnitofWork;
using SocialX.Core.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialX.Core.Domain.Services
{
    public class FollowService : IFollowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationServices _notificationService;

        public FollowService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            INotificationServices notificationService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        public async Task<bool> FollowUserAsync(
            Guid followerId,
            Guid followingId,
            CancellationToken cancellationToken = default)
        {
            // Validate input
            if (followerId == Guid.Empty || followingId == Guid.Empty)
                throw new ArgumentException("Follower ID and Following ID cannot be empty");

            if (followerId == followingId)
                throw new InvalidOperationException("A user cannot follow themselves");

            try
            {
                var existingFollow = await _unitOfWork.Repository<Follow>()
                    .FindAsync(
                        f => f.FollowerId == followerId && f.FollowingId == followingId,
                        cancellationToken: cancellationToken);

                if (existingFollow != null)
                    return false;

                var follow = new Follow
                {
                    FollowerId = followerId,
                    FollowingId = followingId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<Follow>()
                    .AddAsync(follow, cancellationToken);

                await _unitOfWork.CompleteAsync(cancellationToken);

                // 🔔 Fire Notification (Side Effect)
                try
                {
                    await _notificationService.CreateNotificationAsync(
                        new CreateNotificationDto
                        {
                            UserId = followingId,        // Receiver
                            ActorUserId = followerId,    // Actor
                            Type = NotificationType.Follow,
                            EntityId = followerId,
                            Content = "started following you"
                        },
                        cancellationToken);
                }
                catch
                {
                    // log only – notification failure must not break follow
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while following user", ex);
            }
        }

        public async Task<bool> UnfollowUserAsync(
            Guid followerId,
            Guid followingId,
            CancellationToken cancellationToken = default)
        {
            if (followerId == Guid.Empty || followingId == Guid.Empty)
                throw new ArgumentException("Follower ID and Following ID cannot be empty");

            try
            {
                var follow = await _unitOfWork.Repository<Follow>()
                    .FindAsync(
                        f => f.FollowerId == followerId && f.FollowingId == followingId,
                        cancellationToken: cancellationToken);

                if (follow == null)
                    return false;

                _unitOfWork.Repository<Follow>().Delete(follow);
                await _unitOfWork.CompleteAsync(cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while unfollowing user", ex);
            }
        }

        public async Task<bool> IsFollowingAsync(
            Guid followerId,
            Guid followingId,
            CancellationToken cancellationToken = default)
        {
            if (followerId == Guid.Empty || followingId == Guid.Empty)
                throw new ArgumentException("Follower ID and Following ID cannot be empty");

            var follow = await _unitOfWork.Repository<Follow>()
                .FindAsync(
                    f => f.FollowerId == followerId && f.FollowingId == followingId,
                    cancellationToken: cancellationToken);

            return follow != null;
        }

        public async Task<IEnumerable<FollowerDto>> GetFollowersAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            var followers = await _unitOfWork.Repository<Follow>()
                .FindAllAsync(
                    f => f.FollowingId == userId,
                    includeProperties: "Follower,Following",
                    cancellationToken: cancellationToken);

            return _mapper.Map<IEnumerable<FollowerDto>>(followers);
        }

        public async Task<IEnumerable<FollowingDto>> GetFollowingAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            var following = await _unitOfWork.Repository<Follow>()
                .FindAllAsync(
                    f => f.FollowerId == userId,
                    includeProperties: "Follower,Following",
                    cancellationToken: cancellationToken);

            return _mapper.Map<IEnumerable<FollowingDto>>(following);
        }

        public async Task<int> GetFollowersCountAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            return await _unitOfWork.Repository<Follow>()
                .CountAsync(f => f.FollowingId == userId, cancellationToken);
        }

        public async Task<int> GetFollowingCountAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty");

            return await _unitOfWork.Repository<Follow>()
                .CountAsync(f => f.FollowerId == userId, cancellationToken);
        }

        public async Task<FollowDto> GetFollowDetailsAsync(
            Guid followerId,
            Guid followingId,
            CancellationToken cancellationToken = default)
        {
            if (followerId == Guid.Empty || followingId == Guid.Empty)
                throw new ArgumentException("Follower ID and Following ID cannot be empty");

            var follow = await _unitOfWork.Repository<Follow>()
                .FindAsync(
                    f => f.FollowerId == followerId && f.FollowingId == followingId,
                    includeProperties: "Follower,Following",
                    cancellationToken: cancellationToken);

            if (follow == null)
                return null;

            return _mapper.Map<FollowDto>(follow);
        }
    }
}
