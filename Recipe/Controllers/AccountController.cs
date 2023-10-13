using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AutoMapper;

using Application.DTOs.Response;
using Core.Interfaces.Repositories;
using Core.CustomModels;
using Core.Common;
using Application.Interfaces;
using Application.DTOs.Request;

namespace RecipeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly INotificationsRepository _nofificationsRepository;
        private readonly IUserSession _session;
        private readonly IMapper _mapper;

        public AccountController(
            IRecipeRepository recipeRepository,
            INotificationsRepository nofificationsRepository,
            IUserSession session,
            IMapper mapper
            )
        {
            _recipeRepository = recipeRepository;
            _session = session;
            _mapper = mapper;
            _nofificationsRepository = nofificationsRepository;
        }

        [HttpGet("my-recipes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<PaginatedList<RecipeSummary>> GetRelatedRecipes(
            [FromQuery] PaginatedRequest request
            )
        {
            var res = await _recipeRepository.GetRecipesByUsername(
                _session.UserId,
                _session.Username,
                request.CurrentPage,
                request.PageSize
                );

            return res;
        }


        [HttpGet("favourites")]
        public async Task<PaginatedList<RecipeSummary>> Favourites(
            [FromQuery] PaginatedRequest request
            )
        {
            var res = await _recipeRepository.GetFavourites(
                _session.UserId,
                request.CurrentPage,
                request.PageSize
                );

            return res;
        }


        [HttpGet("recent-notifications")]
        public async Task<PaginatedList<NotificationResponse>> GetNotifications(
            [FromQuery] PaginatedRequest request
            )
        {
            var res = await _nofificationsRepository.GetRecentNotifications(
                _session.UserId,
                request.CurrentPage,
                request.PageSize
                );

            return _mapper.Map<PaginatedList<NotificationResponse>>( res );
        }

        [HttpPost("read-notifications")]
        public async Task<IActionResult> ReadNotifications()
        {
            await _nofificationsRepository.ReadRecentNotifications(_session.UserId);

            return NoContent();
        }
    }
}
