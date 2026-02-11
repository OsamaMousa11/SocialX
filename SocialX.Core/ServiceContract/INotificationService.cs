using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.NotificationDto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface INotificationServices
    {
        /// <summary>
        /// Get unread notifications count for a user
        /// </summary>
        Task<int> GetUnreadNotificationCountAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create new notification
        /// </summary>
        Task<NotificationDto> CreateNotificationAsync(
            CreateNotificationDto notificationAddRequest,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Mark single notification as read
        /// </summary>
        Task MarkAsReadAsync(
            Guid notificationId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Mark all notifications as read for user
        /// </summary>
        Task MarkAllAsReadAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete notification
        /// </summary>
        Task<bool> DeleteAsync(
            Guid notificationId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get single notification by condition
        /// </summary>
        Task<NotificationDto?> GetByAsync(
            Expression<Func<Notification, bool>> predicate,
            bool isTracked = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get notifications list (with optional filter + paging)
        /// </summary>
        Task<IEnumerable<NotificationDto>> GetAllAsync(
            Expression<Func<Notification, bool>>? predicate = null,
            int pageIndex = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default);
    }
}
