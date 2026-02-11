using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SocialX.Core.Domain.Entites;
using SocialX.Core.DTO.NotificationDto;
using SocialX.Core.Hubs;
using SocialX.Core.IUnitofWork;
using SocialX.Core.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SocialX.Core.Service
{
    public class NotificationService : INotificationServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // ================= CREATE =================
        public async Task<NotificationDto> CreateNotificationAsync(
            CreateNotificationDto dto,
            CancellationToken cancellationToken = default)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.UserId == Guid.Empty || dto.ActorUserId == Guid.Empty)
                throw new ArgumentException("Invalid user id");

            if (dto.UserId == dto.ActorUserId)
                throw new InvalidOperationException("Cannot send notification to yourself");

            var notification = _mapper.Map<Notification>(dto);

            notification.Id = Guid.NewGuid();
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;

            await _unitOfWork.Repository<Notification>()
        .AddAsync(notification, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

           
            var fullNotification = await _unitOfWork.Repository<Notification>()
                .FindAsync(
                    n => n.Id == notification.Id,
                    includeProperties: "SenderUser,ReceiverUser",
                    cancellationToken: cancellationToken
                );

            await _hubContext.Clients
                .Group($"USER_{notification.UserId}")
                .SendAsync(
                    "ReceiveNotification",
                    _mapper.Map<NotificationDto>(fullNotification),
                    cancellationToken);


            return _mapper.Map<NotificationDto>(notification);
        }

     
        public async Task<int> GetUnreadNotificationCountAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid user id");

            return await _unitOfWork.Repository<Notification>()
                .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);
        }

        public async Task<NotificationDto?> GetByAsync(
            Expression<Func<Notification, bool>> predicate,
            bool isTracked = true,
            CancellationToken cancellationToken = default)
        {
            var notification = await _unitOfWork.Repository<Notification>()
                .FindAsync(
                    predicate,
                    includeProperties: "SenderUser,ReceiverUser",
                    cancellationToken: cancellationToken);

            return notification == null
                ? null
                : _mapper.Map<NotificationDto>(notification);
        }

        // ================= GET ALL =================
        public async Task<IEnumerable<NotificationDto>> GetAllAsync(
            Expression<Func<Notification, bool>>? predicate = null,
            int pageIndex = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (pageIndex < 1 || pageSize < 1)
                throw new ArgumentException("Invalid paging parameters");

            var notifications = await _unitOfWork.Repository<Notification>()
                .GetPagedAsync(
                    pageSize: pageSize,
                    predicate: predicate,
                    orderBy: q => q.OrderByDescending(n => n.CreatedAt),
                    includeProperties: "SenderUser,ReceiverUser",
                    cancellationToken: cancellationToken);

            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }

        // ================= MARK ONE =================
        public async Task MarkAsReadAsync(
            Guid notificationId,
            CancellationToken cancellationToken = default)
        {
            if (notificationId == Guid.Empty)
                throw new ArgumentException("Invalid notification id");

            var notification = await _unitOfWork.Repository<Notification>()
                .GetByIdAsync(notificationId);

            if (notification == null)
                throw new KeyNotFoundException("Notification not found");

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                notification.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Repository<Notification>().Update(notification);
                await _unitOfWork.CompleteAsync(cancellationToken);
            }
        }

        // ================= MARK ALL =================
        public async Task MarkAllAsReadAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Invalid user id");

            var notifications = await _unitOfWork.Repository<Notification>()
                .FindAllAsync(
                    n => n.UserId == userId && !n.IsRead
                    );

            if (!notifications.Any())
                return;

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.UpdatedAt = DateTime.UtcNow;
            }

            _unitOfWork.Repository<Notification>().UpdateRange(notifications);
            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        // ================= DELETE =================
        public async Task<bool> DeleteAsync(
            Guid notificationId,
            CancellationToken cancellationToken = default)
        {
            if (notificationId == Guid.Empty)
                throw new ArgumentException("Invalid notification id");

            var notification = await _unitOfWork.Repository<Notification>()
                .GetByIdAsync(notificationId);

            if (notification == null)
                return false;

            _unitOfWork.Repository<Notification>().Delete(notification);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return true;
        }
    }
}
