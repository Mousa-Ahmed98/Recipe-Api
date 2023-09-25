using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using Infrastructure.CustomModels;
using Infrastructure.Repositories.Interfaces;

using Application.DTOs.Response;
using Application.UserSession;
using RecipeAPI.DTOs.Request.Common;

namespace RecipeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly INotificationsRepository NofificationsRepository;
        private readonly IUserSession _session;
        private readonly IMapper _mapper;

        public AccountController(
            IRecipeRepository recipeRepository,
            INotificationsRepository nofificationsRepository,
            IUserSession session,
            IMapper mapper
            )
        {
            this.recipeRepository = recipeRepository;
            _session = session;
            _mapper = mapper;
            NofificationsRepository = nofificationsRepository;
        }


        [HttpGet("my-recipes")]
        public async Task<PaginatedList<RecipeSummary>> GetRecipesByUsername(
            [FromQuery] PaginatedRequest request
            )
        {
            var res = await recipeRepository.GetRecipesByUsername(
                _session.Username,
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
            var res = await NofificationsRepository.GetRecentNotifications(
                _session.UserId,
                request.CurrentPage,
                request.PageSize
                );

            return _mapper.Map<PaginatedList<NotificationResponse>>( res );
        }

        [HttpPost("read-notifications")]
        public async Task<IActionResult> ReadNotifications()
        {
            await NofificationsRepository.ReadRecentNotifications(_session.UserId);

            return NoContent();
        }
    }
}
