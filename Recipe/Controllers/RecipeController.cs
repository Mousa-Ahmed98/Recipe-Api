using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

using Core.Common;
using Core.CustomModels;
using Infrastructure.Exceptions;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.DomainServcies;

namespace RecipeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipesService _recipeService;
        private readonly IRatingsService _ratingsService;

        public RecipeController(
            IRecipesService recipeService,
            IRatingsService ratingsService
            )
        {
            _recipeService = recipeService;
            _ratingsService = ratingsService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<PaginatedList<RecipeSummary>> GetAll(
            [FromQuery] GetRecipeRequest request
            )
        {
            var res = await _recipeService.GetRecipesSummary(request);

            return res;
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecipeResponse>> GetById(int id)
        {
            try
            {
                var res = await _recipeService.GetOneById(id);

                return Ok( res );

            }catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRecipe([FromBody] RecipeRequest recipeRequest)
        {
            try
            {
                // soon :: FluentValidation
                string validationMessage = recipeRequest.Validata();
                if (validationMessage == "")
                {
                    var NewRecipe = await _recipeService.CreateNewRecipe(recipeRequest);
                
                    return CreatedAtAction(nameof(AddRecipe), NewRecipe);
                }
                else
                {
                    return BadRequest(validationMessage);
                }
            }
            catch (BadRequestException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeRequest recipeRequest)
        {
            try
            {
                var validationStr = recipeRequest.Validata();
                if(validationStr != "") return BadRequest(validationStr);

                var res = await _recipeService.UpdateRecipe(id, recipeRequest);

                return CreatedAtAction(nameof(UpdateRecipe), res);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnAuthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            try
            {
                await _recipeService.RemoveRecipeById(id);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnAuthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
        }


        [HttpGet("filter")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedList<RecipeSummary>>> GetFilteredRecipes(
            [FromQuery] FilteredRecipeRequest request
            )
        {
            var res = await _recipeService.FilterByIngredients(request);
            
            return Ok(res);
        }
        

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedList<RecipeSummary>>> Search(
            [FromQuery] string query, [FromQuery] PaginatedRequest request
            )
        {
            var res = await _recipeService.SearchRecipes(query, request);

            return Ok(res);
        }


        [HttpPost("{id}/favourites")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToFavourites(
            [FromRoute] int id
            )
        {
            try 
            { 
                await _recipeService.AddRecipeToFavourites(id);
                
                return Created(nameof(AddToFavourites), null);
            }
            catch(NotFoundException ex) 
            {
                return NotFound(ex.Message);  
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}/favourites")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveFromFavourites(
            [FromRoute] int id
            )
        {
            try
            {
                await _recipeService.RemoveRecipeFromFavourites(id);

                return Created(nameof(RemoveFromFavourites), null);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// 
        /// Recipe Rating
        /// 

        [AllowAnonymous]
        [HttpGet("{id}/rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRating(
            [FromRoute] int id, [FromQuery] PaginatedRequest request
            )
        {
            try
            {
                var res = await _ratingsService.GetRatings(id, request);

                return Ok(res);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("{id}/rating")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRating(
            [FromRoute] int id, [FromBody] RatingRequest request
            )
        {
            try
            {
                var res = await _ratingsService.AddRating(id, request);

                return Created(nameof(AddRating), res);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}/rating")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRating(
            [FromRoute] int id, [FromBody] RatingRequest request
            )
        {
            try
            {
                var res = await _ratingsService.UpdateRating(id, request);

                return Created(nameof(UpdateRating), res);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnAuthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}/rating")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveRating(
            [FromRoute] int id
            )
        {
            try
            {
                await _ratingsService.RemoveRating(id);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnAuthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
