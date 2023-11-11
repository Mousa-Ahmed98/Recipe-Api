using Application.DTOs.Request;
using System.Threading.Tasks;

using Core.Common;
using Application.DTOs.Response;

namespace Application.Interfaces.DomainServcies
{
    public interface IRatingsService
    {
        Task<PaginatedList<RatingResponse>> GetRatings(int recipeId, PaginatedRequest request);
        Task<RatingResponse> AddRating(int recipeId, RatingRequest request);
        Task<RatingResponse> UpdateRating(int recipeId, RatingRequest request);
        Task RemoveRating(int recipeId);
    }
}
