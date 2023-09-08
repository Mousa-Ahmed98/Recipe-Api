using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CoreEntities = Core.Entities;
using RecipeAPI.DTOs.Response;
using Core.Entities;
using Recipe.DTOs.Request;
using Core.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {

        private readonly IRecipeRepository recipeRepository;
        private readonly IBaseRepository<Ingredient> ingredientRepository;
        private readonly IBaseRepository<Step> stepRepository;
        private readonly IMapper _mapper;


        public RecipeController(IRecipeRepository recipeRepository
            , IBaseRepository<Ingredient> ingredientRepository
            , IBaseRepository<Step> stepRepository
            , IMapper mapper)
        {
            this.recipeRepository = recipeRepository;
            this.ingredientRepository = ingredientRepository;
            this.stepRepository = stepRepository;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<RecipeResponse>> GetAll()
        {
            var res = await recipeRepository.GetAllRecipes();
            
            return _mapper.Map<IEnumerable<RecipeResponse>> (res);
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

            try
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }




        [HttpPut("Update/{id}")]
        public IActionResult UpdateRecipe(int id, [FromBody] RecipeRequest recipeDto)
        {

            var existingRecipe = recipeRepository.GetOneById(id).Result;

            if (existingRecipe == null) return NotFound("Recipe not found");

            if (recipeDto == null) return BadRequest("Invalid request data. recipeDto is null.");

            deleteIngredientsAndSteps(existingRecipe);
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
            deleteIngredientsAndSteps(recipe);
            return Ok(recipe);

        }

        private void deleteIngredientsAndSteps(CoreEntities.Recipe existingRecipe)
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
