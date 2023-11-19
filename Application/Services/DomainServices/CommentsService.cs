using System.Threading.Tasks;
using AutoMapper;

using Core.Enums;
using Core.Entities;
using Core.Interfaces;
using Core.Common;
using Core.CustomModels;

using Infrastructure.Exceptions.Recipe;
using Infrastructure.Exceptions;

using Application.Interfaces;
using Application.Interfaces.DomainServices;
using Application.DTOs.Request;
using Application.DTOs.Response;



namespace Application.Services.DomainServices
{
    public class CommentsService : ICommentsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserSession _session;
        private readonly INotificationManager _notificationManager;

        public CommentsService(
            IUserSession session,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            INotificationManager notificationManager
            )
        {
            _session = session;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationManager = notificationManager;
        }

        /// 
        /// *** Comments *** 
        /// 

        public async Task<PaginatedList<CommentResponse>> GetComments(
            int recipeId, PaginatedRequest request
            )
        {
            var comments = await _unitOfWork.CommentsRepository
                .GetComments(recipeId, request.PageNumber, request.PageSize);

            return _mapper.Map<PaginatedList<CommentResponse>>(comments);
        }

        public async Task<CommentResponse> AddComment(CommentRequest request)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetByIdAsync(request.RecipeId);

            if (recipe == null)
            {
                throw new RecipeNotFoundException(request.RecipeId);
            }

            Comment comment = new Comment()
            {
                RecipeId = request.RecipeId,
                UserId = _session.UserId,
                Content = request.Content
            };

            // Notify the author about the new comment they've just recieved.
            _ = _notificationManager.CreateOne(
                    userId: recipe.AuthorId,
                    recipe: _mapper.Map<RecipeSummary>(recipe),
                    type: NotificationType.Comment
                );

            await _unitOfWork.CommentsRepository.AddAsync(comment);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<CommentResponse>(comment);
        }

        public async Task<CommentResponse> UpdateComment(int commentId, CommentRequest request)
        {
            var comment = await _unitOfWork.CommentsRepository.GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new NotFoundException("Comment Was Not Found");
            }

            if (comment.UserId != _session.UserId)
            {
                throw new UnAuthorizedException();
            }

            comment.Content = request.Content;
            _unitOfWork.CommentsRepository.Update(comment);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<CommentResponse>(comment);
        }

        public async Task DeleteComment(int commentId)
        {
            var comment = await _unitOfWork.CommentsRepository.GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new NotFoundException("Comment Was Not Found");
            }

            if (comment.UserId != _session.UserId)
            {
                throw new UnAuthorizedException();
            }

            _unitOfWork.CommentsRepository.Delete(comment);
            await _unitOfWork.SaveAsync();
        }

        /// 
        /// *** Replies *** 
        /// 

        public async Task<ReplyResponse> AddReply(int commentId, ReplyRequest request)
        {
            var comment = await _unitOfWork.CommentsRepository.GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new NotFoundException("Comment Was Not Found");
            }

            Reply reply = new Reply()
            {
                CommentId = comment.Id,
                UserId = _session.UserId,
                Content = request.Content
            };

            await _unitOfWork.RepliesRepository.AddAsync(reply);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReplyResponse>(reply);
        }

        public async Task<ReplyResponse> UpdateReply(int replyId, ReplyRequest request)
        {
            var reply = await _unitOfWork.RepliesRepository.GetByIdAsync(replyId);

            if (reply == null)
            {
                throw new NotFoundException("Reply Was Not Found");
            }

            if (reply.UserId != _session.UserId)
            {
                throw new UnAuthorizedException();
            }

            reply.Content = request.Content;
            _unitOfWork.RepliesRepository.Update(reply);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReplyResponse>(reply);
        }

        public async Task DeleteReply(int replyId)
        {
            var reply = await _unitOfWork.RepliesRepository.GetByIdAsync(replyId);

            if (reply == null)
            {
                throw new NotFoundException("Comment Was Not Found");
            }

            if (reply.UserId != _session.UserId)
            {
                throw new UnAuthorizedException();
            }

            _unitOfWork.RepliesRepository.Delete(reply);
            await _unitOfWork.SaveAsync();
        }

    }
}
