using SocialX.Core.DTO.FollowDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface IFollowService
    {
        /// <summary>
        /// Create a follow relationship
        /// </summary>
        Task<bool> FollowUserAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unfollow a user
        /// </summary>
        Task<bool> UnfollowUserAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if a user follows another user
        /// </summary>
        Task<bool> IsFollowingAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all followers of a user
        /// </summary>
        Task<IEnumerable<FollowerDto>> GetFollowersAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all users that a user is following
        /// </summary>
        Task<IEnumerable<FollowingDto>> GetFollowingAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get followers count
        /// </summary>
        Task<int> GetFollowersCountAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get following count
        /// </summary>
        Task<int> GetFollowingCountAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get follow details
        /// </summary>
        Task<FollowDto> GetFollowDetailsAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default);
    }
}
