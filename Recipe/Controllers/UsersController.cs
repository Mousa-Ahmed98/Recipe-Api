using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Core.CustomModels;
using Infrastructure.Exceptions;
using Application.DTOs.Request;
using Application.Interfaces.DomainServices;
using Application.Interfaces.DomainServcies;

namespace RecipeApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRecipesService _recipeService;

        public UsersController(
            IUserService userService,
            IRecipesService recipeService
            )
        {
            _userService = userService;
            _recipeService = recipeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] PaginatedRequest request
            )
        {
            try
            {
                var res = await _userService.GetUsers(request);

                return Ok(res);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetByUsername([FromRoute] string username)
        {
            try
            {
                var res = await _userService.GetByUsername(username);

                return Ok(res);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("{username}/follow")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Follow([FromRoute] string username)
        {
            try
            {
                await _userService.FollowUser(username);
            
                return CreatedAtAction(nameof(Follow), null);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("{username}/unfollow")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UnFollow([FromRoute] string username)
        {
            try
            {
                await _userService.UnfollowUser(username);

                return CreatedAtAction(nameof(UnFollow), null);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{username}/recipes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserRecipes(
            [FromRoute] string username,
            [FromQuery] PaginatedRequest request
            )
        {
            try
            {
                var res = await _recipeService.GetRecipesByUsername(username, request);
                
                return Ok(res);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            } 
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}