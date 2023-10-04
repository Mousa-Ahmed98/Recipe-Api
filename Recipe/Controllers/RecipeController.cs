using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.CustomModels;
using Infrastructure.Exceptions;

using Application.UserSession;
using Application.Interfaces;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.DTOs.Request.Common;

namespace RecipeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IBaseRepository<Ingredient> _ingredientRepository;
        private readonly IBaseRepository<Step> _stepRepository;
        private readonly IMapper _mapper;
        private readonly IUserSession _session;
        private readonly IImageService _imageService;

        public RecipeController(IRecipeRepository recipeRepository,
            IBaseRepository<Ingredient> ingredientRepository,
            IBaseRepository<Step> stepRepository,
            IMapper mapper,
            IBaseRepository<Category> categoryRepository,
            IUserSession session,
            IImageService imageService
            )
        {
            _recipeRepository = recipeRepository;
            _ingredientRepository = ingredientRepository;
            _stepRepository = stepRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _session = session;
            _recipeRepository.SetUserId(_session.UserId);
            _imageService = imageService;
        }

        [HttpGet]
        [AllowAnonymous]
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
        public async Task<ActionResult<RecipeResponse>> GetById(int id)
        {
            var res = await _recipeRepository.GetOneById(id);

            if (res == null) return NotFound();

            return _mapper.Map<RecipeResponse>(res);
        }

        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<PaginatedList<RecipeSummary>> GetFilteredRecipes(
            [FromQuery] FilteredRecipeRequest request
            )
        {
            List<string> filterIngredients = request.Ingredients.Split(',').ToList();

            var res = await _recipeRepository.FilterByIngredients(
                request.CurrentPage,
                request.PageSize,
                filterIngredients
                );

            return res;
        }

        [HttpGet("search")]
        public async Task<PaginatedList<RecipeSummary>> GetFilteredRecipes(
            [FromQuery] string query, [FromQuery] PaginatedRequest request
            )
        {
            var res = await _recipeRepository.SearchRecipes(
                query, request.CurrentPage, request.PageSize
                );

            return res;
        }

        [Authorize]
        [HttpGet("favourites")]
        public async Task<PaginatedList<RecipeSummary>> Favourites(
            [FromQuery] PaginatedRequest request
            )
        {
            var res = await _recipeRepository.GetFavourites(
                request.CurrentPage,
                request.PageSize
                );

            return res;
        }

        [Authorize]
        [HttpPost("favourites/add/{id}")]
        public async Task<IActionResult> AddToFavourites(
            [FromRoute] int id
            )
        {
            var res = await _recipeRepository.AddRecipeToFavourites(id);

            if (res == false) return NotFound();

            return Ok();
        }


        [HttpDelete("favourites/remove/{id}")]

        public async Task<IActionResult> RemoveFromFavourites(
            [FromRoute] int id
            )
        {
            var res = await _recipeRepository.RemoveRecipeFromFavourites(id);

            if (res == false) return NotFound();

            return NoContent();
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddRecipe([FromBody] RecipeRequest recipeDto)

        {
            try
            {
                recipeDto.appendOrdersToSteps();
                string validationMessage = recipeDto.Validata();
                if (validationMessage == "")
                {
                    var recipe = _mapper.Map<Recipe>(recipeDto);
                    recipe.AuthorId = _session.UserId;
                    
                    if (recipeDto.ImageData != null)
                    {
                        var imageName = await _imageService
                            .SaveImageAsync(recipeDto.ImageData, recipeDto.Name);
                        recipe.ImageName = imageName;
                    }

                    await _recipeRepository.CreateNewRecipe(recipe);

                    return Ok(
                        _mapper.Map<RecipeResponse>(recipe)
                        );
                }
                else
                {
                    return BadRequest(validationMessage);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeRequest recipeDto)
        {

            var existingRecipe = _recipeRepository.GetOneById(id).Result;

            if (existingRecipe == null) return NotFound("Recipe not found");

            if (recipeDto == null) return BadRequest("Invalid request data. recipeDto is null.");

            var category = (await _categoryRepository
                .GetAsync(x => x.Id == recipeDto.CategoryId))
                .FirstOrDefault();

            if (category == null)
            {
                return BadRequest("Invalid Category");
            }
            existingRecipe.Category = category;

            DeleteIngredientsAndSteps(existingRecipe);
            recipeDto.applyUpdateChanges(existingRecipe);

            _recipeRepository.Update(existingRecipe);
            return Ok(existingRecipe);
        }

        [HttpDelete("Delete/{id}")]
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void DeleteIngredientsAndSteps(Recipe existingRecipe)
        {
            var ings = existingRecipe.Ingredients;
            foreach (var item in ings)
            {
                _ingredientRepository.Delete(item);
            }
            var stps = existingRecipe.Steps;
            foreach (var item in stps)
            {
                _stepRepository.Delete(item);
            }
        }
    }
}
