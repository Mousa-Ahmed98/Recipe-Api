using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CoreEntities = Core.Entities;
using RecipeAPI.DTOs.Response;
using Core.Entities;
using Recipe.DTOs.Request;
using Core.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IBaseRepository<Category> categoryRepository;
        private readonly IBaseRepository<Ingredient> ingredientRepository;
        private readonly IBaseRepository<Step> stepRepository;
        private readonly IBaseRepository<Review> reviewRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public RecipeController(IRecipeRepository recipeRepository
            , IBaseRepository<Ingredient> ingredientRepository
            , IBaseRepository<Step> stepRepository
            , IMapper mapper
            , IBaseRepository<Category> categoryRepository
            , IBaseRepository<Review> reviewRepository
            ,UserManager<ApplicationUser> userManager)
        {
            this.recipeRepository = recipeRepository;
            this.ingredientRepository = ingredientRepository;
            this.stepRepository = stepRepository;
            _mapper = mapper;
            this.categoryRepository = categoryRepository;
            this.reviewRepository = reviewRepository;
            _userManager = userManager;
        }
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await recipeRepository.GetAllRecipes();
            
            var result = _mapper.Map<IEnumerable<RecipeResponse>>(res);
            return Ok(result);

            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeResponse>> GetById(int id)
        {
            var res = await recipeRepository.GetOneById(id);
            
            if (res == null) return NotFound();
            
            return _mapper.Map<RecipeResponse>(res);
        }

        [HttpPost("Add")]

        public async Task<IActionResult> AddRecipe([FromBody] RecipeRequest recipeDto)

        {
                recipeDto.appendOrdersToSteps();
                string validationMessage = recipeDto.Validata();
                if (validationMessage=="")
                {
                    var recipe = _mapper.Map<CoreEntities.Recipe>(recipeDto);
                    recipeRepository.Add(recipe);
                    await recipeRepository.SaveChangesAsync();

                    return Ok(recipe);
                }
                else
                {
                    return BadRequest(validationMessage);
                }
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
            
            if(category == null)
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
            Review review = new Review {
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
            Review review = await reviewRepository.FindByIdAsync(updatedReviewDto.Id);
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
        public async Task<IActionResult> UpdateReview(int id)
        {

            Review review = await reviewRepository.FindByIdAsync(id);
            if (review is not null)
            {
                reviewRepository.Delete(review);
                return Ok(review);
            }
            return NotFound("Review not found.");


        }





        private void DeleteIngredientsAndSteps(CoreEntities.Recipe existingRecipe)
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
