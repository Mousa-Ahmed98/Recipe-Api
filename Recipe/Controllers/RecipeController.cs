using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.CustomModels;
using Infrastructure.Exceptions;
using Infrastructure.Exceptions.Recipe;

using Application.UserSession;
using Application.Interfaces;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.DTOs.Request.Common;
using Microsoft.AspNetCore.Http;

namespace RecipeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUserSession _session;
        private readonly IImageService _imageService;

        public RecipeController(IRecipeRepository recipeRepository,
            IBaseRepository<Category> categoryRepository,
            IImageService imageService,
            IUserSession session,
            IMapper mapper
            )
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _session = session;
            if (_session.IsAuthenticated)
            {
                _recipeRepository.SetUserId(_session.UserId);
            }
            _imageService = imageService;
        }


        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<PaginatedList<RecipeSummary>> GetAll(
            [FromQuery] GetRecipeRequest request
            )
        {
            var res = await _recipeRepository.GetRecipesSummary(
                request.CurrentPage,
                request.PageSize,
                request.Category
                );

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
                var res = await _recipeRepository.GetOneById(id);

                return Ok( 
                    _mapper.Map<RecipeResponse>(res)
                    );

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
            // soon :: FluentValidation
            string validationMessage = recipeRequest.Validata();
            if (validationMessage == "")
            {
                var recipe = _mapper.Map<Recipe>(recipeRequest);
                recipe.AuthorId = _session.UserId;

                if (recipeRequest.ImageData != null)
                {
                    var imageName = await _imageService
                        .SaveImageAsync(recipeRequest.ImageData, recipeRequest.Name);
                    recipe.ImageName = imageName;
                }

                var newRecipe = await _recipeRepository.CreateNewRecipe(recipe);

                return CreatedAtAction(
                    nameof(AddRecipe), _mapper.Map<RecipeResponse>(newRecipe)
                    );
            }
            else
            {
                return BadRequest(validationMessage);
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

                var existingRecipe = await _recipeRepository.GetById(id);

                if (existingRecipe == null)
                {
                    throw new RecipeNotFoundException(id);
                }

                if (existingRecipe.AuthorId != _session.UserId)
                {
                    throw new UnAuthorizedException();
                }

                var updatedRecipe = _mapper.Map<Recipe>(recipeRequest);

                if (!string.IsNullOrEmpty(recipeRequest.ImageData))
                {
                    var imageName = await _imageService
                        .SaveImageAsync(recipeRequest.ImageData, recipeRequest.Name);
                    updatedRecipe.ImageName = imageName;
                }
                else
                {
                    updatedRecipe.ImageName = existingRecipe.ImageName;
                }
                
                updatedRecipe.Id = id;
                updatedRecipe.AuthorId = _session.UserId;
                var res = await _recipeRepository.UpdateRecipe(updatedRecipe);

                return CreatedAtAction(
                    nameof(UpdateRecipe), _mapper.Map<RecipeResponse>(res)
                    );
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return NotFound(ex.Message);
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
                await _recipeRepository.RemoveRecipeById(id);

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
            List<string> filterIngredients = request.Ingredients.Split(',').ToList();

            var res = await _recipeRepository.FilterByIngredients(
                request.CurrentPage,
                request.PageSize,
                filterIngredients
                );

            return Ok(res);
        }
        

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedList<RecipeSummary>>> Search(
            [FromQuery] string query, [FromQuery] PaginatedRequest request
            )
        {
            var res = await _recipeRepository.SearchRecipes(
                query, request.CurrentPage, request.PageSize
                );

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
                await _recipeRepository.AddRecipeToFavourites(id);
                
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
                await _recipeRepository.RemoveRecipeFromFavourites(id);

                return Created(nameof(AddToFavourites), null);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
