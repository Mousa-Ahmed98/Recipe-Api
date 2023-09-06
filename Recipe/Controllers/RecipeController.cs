using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreEntities = Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Core.Entities;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using System.Diagnostics;
using Recipe.DTOs.Request;
using Recipe.DTOs.Response;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IBaseRepository<CoreEntities.Recipe> recipeRepository;
        private readonly IBaseRepository<Ingredient> ingredientRepository;
        private readonly IBaseRepository<Step> stepRepository;
        private readonly IMapper _mapper;

        public RecipeController(IBaseRepository<CoreEntities.Recipe> recipeRepository
            , IBaseRepository<Ingredient> ingredientRepository
            , IBaseRepository<Step> stepRepository
            , IMapper mapper)
        {
            this.recipeRepository = recipeRepository;
            this.ingredientRepository = ingredientRepository;
            this.stepRepository = stepRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<RecipeResponse>> GetAll()
        {
            var res = await recipeRepository.GetAll();
            
            return _mapper.Map<IEnumerable<RecipeResponse>> (res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeResponse>> GetById(int id)
        {
            var res = recipeRepository.Get(
                c => c.Id == id,
                includeProperties: "Ingredients, Steps")
                .FirstOrDefault();
            if (res == null) return NotFound();
            
            return _mapper.Map<RecipeResponse>(res);
        }

        [HttpPost("Add")]
        public IActionResult AddRecipe([FromBody] RecipeRequest recipeDto)
        {

            try
            {
                recipeDto.appendOrdersToSteps();
                string validationMessage = recipeDto.Validata();
                if (validationMessage=="")
                {
                    var recipe = _mapper.Map<CoreEntities.Recipe>(recipeDto);
                    recipeRepository.Add(recipe);
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

            var existingRecipe = recipeRepository.Get(
            c => c.Id == id,includeProperties: "Ingredients, Steps")
            .FirstOrDefault();

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
            var recipe = recipeRepository.Get(r => r.Id == id, includeProperties: "Ingredients, Steps").FirstOrDefault();
            if (recipe == null) return NotFound("Recipe not found");
            deleteIngredientsAndSteps(recipe);
            recipeRepository.Delete(recipe);
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
