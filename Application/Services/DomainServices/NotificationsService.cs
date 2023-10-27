using System.Threading.Tasks;
using AutoMapper;

using Application.Interfaces;
using Application.DTOs.Response;
using Application.Interfaces.DomainServices;
using Core.Interfaces;
using Core.Common;
using Application.DTOs.Request;

namespace Application.Services.DomainServices
{
    public class NotificationsService : INotificationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserSession _session;
        private readonly IMapper _mapper;

        public NotificationsService(
            IUserSession session,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _session = session;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<NotificationResponse>> GetRecentNotifications(PaginatedRequest request)
        {
            var notifications = await _unitOfWork.NotificationsRepository
                .GetRecentNotifications(_session.UserId, request.PageNumber, request.PageSize);

            return _mapper.Map<PaginatedList<NotificationResponse>>(notifications);
        }

        public async Task ReadRecentNotifications()
        {
            await _unitOfWork.NotificationsRepository
                .ReadRecentNotifications(_session.UserId);
        }
    }
}
