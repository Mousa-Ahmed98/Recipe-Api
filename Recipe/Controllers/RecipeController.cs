using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CoreEntities = Core.Entities;
using RecipeAPI.DTOs.Response;
using Core.Entities;
using RecipeApi.DTOs.Request;
using Core.Interfaces;
using Infrastructure.Repositories.Interfaces;
using RecipeAPI.DTOs.Request;
using Infrastructure.CustomModels;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using Application.UserSession;
using RecipeAPI.DTOs.Request.Common;
using Microsoft.AspNetCore.Identity;
using Application.DTOs.Request;

namespace RecipeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IBaseRepository<Category> categoryRepository;
        private readonly IBaseRepository<Ingredient> ingredientRepository;
        private readonly IBaseRepository<Step> stepRepository;
        private readonly IMapper _mapper;
        private readonly IUserSession _session;
        private readonly IBaseRepository<Review> reviewRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecipeController(IRecipeRepository _recipeRepository
            , IBaseRepository<Ingredient> ingredientRepository
            , IBaseRepository<Step> stepRepository
            , IMapper mapper
            , IBaseRepository<Category> categoryRepository
            , IUserSession session
            , IBaseRepository<Review> reviewRepository
            , UserManager<ApplicationUser> userManager
            )
        {
            this.recipeRepository = _recipeRepository;
            this.ingredientRepository = ingredientRepository;
            this.stepRepository = stepRepository;
            _mapper = mapper;
            this.categoryRepository = categoryRepository;
            _session = session;
            _recipeRepository.SetUserId(_session.UserId);
            this.reviewRepository = reviewRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<PaginatedList<RecipeSummary>> GetAll(
            [FromQuery] GetRecipeRequest request
            )
        {
            var res = await recipeRepository.GetRecipesSummary(
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
            var res = await recipeRepository.GetOneById(id);

            if (res == null) return NotFound();

            return Ok(_mapper.Map<RecipeResponse>(res));
        }

        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<PaginatedList<RecipeSummary>> GetFilteredRecipes(
            [FromQuery] FilteredRecipeRequest request
            )
        {
            List<string> filterIngredients = request.Ingredients.Split(',').ToList();

            var res = await recipeRepository.FilterByIngredients(
                request.CurrentPage,
                request.PageSize,
                filterIngredients
                );

            return res;
        }

        [HttpGet("search")]
        public async Task<PaginatedList<RecipeSummary>> GetFilteredRecipes(
            [FromQuery] string query, 
            [FromQuery] PaginatedRequest request
            )
        {
            var res = await recipeRepository.SearchRecipes(
                query, request.CurrentPage, request.PageSize
                );

            return res;
        }


        [HttpGet("favourites")]
        public async Task<PaginatedList<RecipeSummary>> Favourites(
            [FromQuery] PaginatedRequest request
            )
        {
            var res = await recipeRepository.GetFavourites(
                request.CurrentPage,
                request.PageSize
                );

            return res;
        }

        [HttpPost("favourites/add/{id}")]
        public async Task<IActionResult> AddToFavourites(
            [FromRoute] int id
            )
        {
            var res = await recipeRepository.AddRecipeToFavourites(id);

            if (res == false) return NotFound();

            return Ok();
        }

        [HttpDelete("favourites/remove/{id}")]
        public async Task<IActionResult> RemoveFromFavourites(
            [FromRoute] int id
            )
        {
            var res = await recipeRepository.RemoveRecipeFromFavourites(id);

            if (res == false) return NotFound();

            return NoContent();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddRecipe([FromBody] RecipeRequest recipeDto)

        {
            recipeDto.appendOrdersToSteps();
            string validationMessage = recipeDto.Validata();
            if (validationMessage == "")
            {
                var recipe = _mapper.Map<Recipe>(recipeDto);
                recipeRepository.Add(recipe);
                await recipeRepository.SaveChangesAsync();
                return Ok(recipe);
            }
            return BadRequest(validationMessage);
           
            
            
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeRequest recipeDto)
        {

            var existingRecipe = recipeRepository.GetOneById(id).Result;

            if (existingRecipe == null) return NotFound("Recipe not found");

            if (recipeDto == null) return BadRequest("Invalid request data. recipeDto is null.");

            var category = (await categoryRepository
                .GetAsync(x => x.Id == recipeDto.CategoryId))
                .FirstOrDefault();

            if (category == null)
            {
                return BadRequest("Invalid Category");
            }
            existingRecipe.Category = category;

            DeleteIngredientsAndSteps(existingRecipe);
            
            recipeDto.applyUpdateChanges(existingRecipe);

            recipeRepository.Update(existingRecipe);
            return Ok(existingRecipe);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteRecipe(int id)
        {
            var recipe = recipeRepository.GetOneById(id).Result;
            if (recipe == null) return NotFound("Recipe not found");
            recipeRepository.Delete(recipe);
            DeleteIngredientsAndSteps(recipe);
            return Ok(recipe);

        }

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview(ReviewDto reviewDto)
        {
            var recipe = await recipeRepository.GetOneById(reviewDto.RecipeId);
            if (recipe == null) return NotFound("Recipe does not exist.");
            var user = await _userManager.FindByIdAsync(reviewDto.AuthorId);
            if (user == null) return NotFound($"User with id {reviewDto.AuthorId} is not found.");
            //Implement mapping
            Review review = new Review
            {
                rate = reviewDto.rate,
                content = reviewDto.content,
                AuthorId = reviewDto.AuthorId,
                RecipeId = reviewDto.RecipeId,
                AuthorName = reviewDto.AuthorName
            };
            //recipe.Reviews.Add(review);
            reviewRepository.Add(review);
            return Ok(review);
        }

        [HttpPut("UpdateReview")]
        public async Task<IActionResult> UpdateReview(UpdateReviewDto updatedReviewDto)
        {
            var recipe = await recipeRepository.GetOneById(updatedReviewDto.RecipeId);
            var user = await _userManager.FindByIdAsync(updatedReviewDto.AuthorId);
            if (recipe == null) return NotFound("Recipe does not exist.");
            if (user == null) return NotFound($"User with id {updatedReviewDto.AuthorId} is not found.");
          
            Review review = await reviewRepository.GetById(updatedReviewDto.Id);
            if (review is not null)
            {
                review.content = updatedReviewDto.content;
                review.rate = updatedReviewDto.rate;
                reviewRepository.Update(review);
                return Ok(review);
            }
            return NotFound("Review not found.");
        }

        [HttpDelete("DeleteReview/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {

            Review review = await reviewRepository.GetById(id);
            if (review is not null)
            {
                reviewRepository.Delete(review);
                return Ok(review);
            }
            return NotFound("Review not found.");


        }

        private void DeleteIngredientsAndSteps(Recipe existingRecipe)
        {
            var ings = existingRecipe.Ingredients;
            foreach (var item in ings)
            {
                ingredientRepository.Delete(item);
            }
            var stps = existingRecipe.Steps;
            foreach (var item in stps)
            {
                stepRepository.Delete(item);
            }
        }
    }
}
