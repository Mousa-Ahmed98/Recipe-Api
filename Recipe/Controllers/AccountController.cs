using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Core.CustomModels;
using Core.Common;
using Application.DTOs.Response;
using Application.DTOs.Request;
using Application.Interfaces.DomainServcies;
using Application.Interfaces.DomainServices;

namespace RecipeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRecipesService _recipeService;
        private readonly INotificationsService _nofificationsService;

        public AccountController(
            IRecipesService recipeRepository,
            INotificationsService nofificationsRepository
            )
        {
            _recipeService = recipeRepository;
            _nofificationsService = nofificationsRepository;
        }

        [HttpGet("my-recipes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedList<RecipeSummary>>> GetRelatedRecipes(
            [FromQuery] PaginatedRequest request
            )
        {
            var res = await _recipeService.GetMyRecipesAsync(request);

            return Ok(res);
        }

        [HttpGet("favourites")]
        public async Task<ActionResult<PaginatedList<RecipeSummary>>> Favourites(
            [FromQuery] PaginatedRequest request
            )
        {
            var res = await _recipeService.GetFavourites(request);

            return Ok(res);
        }

        [HttpGet("recent-notifications")]
        public async Task<ActionResult<PaginatedList<NotificationResponse>>> GetNotifications(
            [FromQuery] PaginatedRequest request
            )
        {
            var res = await _nofificationsService.GetRecentNotifications(request);

            return Ok( res );
        }

        [HttpPost("read-notifications")]
        public async Task<IActionResult> ReadNotifications()
        {
            await _nofificationsService.ReadRecentNotifications();

            return NoContent();
        }
    }
}
