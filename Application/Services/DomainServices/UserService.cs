using System.Threading.Tasks;
using AutoMapper;

using Core.Common;
using Core.CustomModels;
using Core.Interfaces;

using Infrastructure.Exceptions.User;

using Application.Interfaces;
using Application.Interfaces.DomainServices;
using Application.DTOs.Request;

namespace Application.Services.DomainServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserSession _session;
        private readonly IMapper _mapper;

        public UserService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserSession userSession
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _session = userSession;
        }

        public async Task<PaginatedList<UserResponse>> GetUsers(PaginatedRequest request)
        {
            var res = await _unitOfWork.UsersRepository
                .GetUsers(_session.UserId, request.PageNumber, request.PageSize);

            return _mapper.Map<PaginatedList<UserResponse>>(res);
        }

        public async Task<UserResponse> GetByUsername(string username)
        {
            var user = await _unitOfWork.UsersRepository.GetByUsername(_session.UserId, username);

            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            return _mapper.Map<UserResponse>(user);
        }

        public async Task FollowUser(string username)
        {
            var followee = await _unitOfWork.UsersRepository.GetByUsername(_session.UserId, username);

            if (followee == null)
            {
                throw new UserNotFoundException(username);
            }

            bool alreadyFollowed = await _unitOfWork.UsersRepository
                .IsFollowedBy(_session.UserId, followee.Id);

            if (alreadyFollowed)
            {
                throw new FollowAlreadyExistsException(username);
            }

            await _unitOfWork.UsersRepository.AddFollow(_session.UserId, followee.Id);
            await _unitOfWork.SaveAsync();
        }

        public async Task UnfollowUser(string username)
        {
            var followee = await _unitOfWork.UsersRepository.GetByUsername(_session.UserId, username);

            if (followee == null)
            {
                throw new UserNotFoundException(username);
            }

            bool followed = await _unitOfWork.UsersRepository
                .IsFollowedBy(_session.UserId, followee.Id);

            if (!followed)
            {
                throw new FollowDoesntExistException(username);
            }

            await _unitOfWork.UsersRepository.RemoveFollow(_session.UserId, followee.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
