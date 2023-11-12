using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Core.Common;

namespace Application.Interfaces.DomainServices
{
    public interface ICommentsService
    {
        Task<PaginatedList<CommentResponse>> GetComments(int recipeId, PaginatedRequest request);
        Task<CommentResponse> AddComment(CommentRequest request);
        Task<CommentResponse> UpdateComment(int commentId, CommentRequest request);
        Task DeleteComment(int commentId);
        
        Task<ReplyResponse> AddReply(int commentId, ReplyRequest request);
        Task<ReplyResponse> UpdateReply(int replyId, ReplyRequest request);
        Task DeleteReply(int replyId);
    }
}