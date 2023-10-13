using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Core.Interfaces.Repositories;
using Infrastructure.Exceptions;
using Application.Interfaces;
using Application.DTOs.Request;

namespace RecipeApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRecipeRepository _RecipeRepository;
        private readonly IUserSession _session;

        public UsersController(
            IUsersRepository usersRepository,
            IUserSession session,
            IRecipeRepository recipeRepository)
        {
            _usersRepository = usersRepository;
            _RecipeRepository = recipeRepository;
            _session = session;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] PaginatedRequest request
            )
        {
            try
            {
                var res = await _usersRepository
                    .GetUsers(_session.UserId, request.CurrentPage, request.PageSize);

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
        public async Task<IActionResult> GetByUsername([FromRoute] string username)
        {
            try
            {
                var res = await _usersRepository.GetByUsername(_session.UserId, username);
            
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
                await _usersRepository.FollowUser(_session.UserId, username);
            
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
                await _usersRepository.UnfollowUser(_session.UserId, username);

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
                var res = await _RecipeRepository
                    .GetRecipesByUsername(
                        _session.UserId,
                        username, 
                        request.CurrentPage, 
                        request.PageSize
                    );

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