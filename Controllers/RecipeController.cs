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
using System.ComponentModel.DataAnnotations;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IBaseRepository<CoreEntities.Recipe> recipeRepository;
        private readonly IMapper _mapper;
        private readonly StoreContext _context;
        private StoreContext? context;

public RecipeController(IBaseRepository<CoreEntities.Recipe> recipeRepository, IMapper mapper, StoreContext context)
{
    this.recipeRepository = recipeRepository;
    _mapper = mapper;
    _context = context; 
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
            var res = await recipeRepository.GetById(id);
            if (res == null) return NotFound();
            
            return _mapper.Map<RecipeResponse>(res);
        }

        [HttpPost("Add")]
        public IActionResult AddRecipe([FromBody] CreateRecipeRequest recipeDto)
        {

            try
            {
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






[HttpPut("Edit/{id}")]
public IActionResult EditRecipe(int id, [FromBody] UpdateRecipeRequest recipeDto)
{
    try
    {
        var existingRecipe = recipeRepository.GetById(id).Result;
        if (existingRecipe == null)
        {
            return NotFound("Recipe not found");
        }

        if (recipeDto == null)
        {
            return BadRequest("Invalid request data. recipeDto is null.");
        }

        if (!string.IsNullOrEmpty(recipeDto.Name))
        {
            existingRecipe.Name = recipeDto.Name;
        }
        
        if (!string.IsNullOrEmpty(recipeDto.Image))
        {
            existingRecipe.Image = recipeDto.Image;
        }


        if (recipeDto.Ingredients != null && recipeDto.Ingredients.Any())
        {
            if (existingRecipe.Ingredients == null)
            {
                existingRecipe.Ingredients = new List<Ingredient>();
            }

            
            existingRecipe.Ingredients.Clear();
            foreach (var ingredientDto in recipeDto.Ingredients)
            {
                existingRecipe.Ingredients.Add(new Ingredient { Description = ingredientDto.Description });
            }
        }

        if (recipeDto.Steps != null && recipeDto.Steps.Any())
        {
            //  Steps is initialized
            if (existingRecipe.Steps == null)
            {
                existingRecipe.Steps = new List<Step>();
            }

            // Update Steps based on recipeDto
            existingRecipe.Steps.Clear();
            foreach (var stepDto in recipeDto.Steps)
            {
                existingRecipe.Steps.Add(new Step { Description = stepDto.Step, Order = (byte)stepDto.StepOrder });
            }
        }

        // Save changes to the database
        recipeRepository.Update(existingRecipe);

        return Ok(existingRecipe);
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}







[HttpDelete("Delete/{id}")]
public async Task<IActionResult> DeleteRecipe(int id, [FromBody] DeleteRequest deleteRequest)
{
    try
    {
        var existingRecipe = await _context.Recipes
            .Include(r => r.Steps)
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (existingRecipe == null)
        {
            return NotFound("Recipe not found");
        }

        if (deleteRequest.DeleteWholeRecipe)
        {
            // Delete the recipe and related entities
            _context.Recipes.Remove(existingRecipe);
        }
        else
        {
            if (deleteRequest.StepIds != null && deleteRequest.StepIds.Any())
            {
                // Remove specific steps
                foreach (var stepId in deleteRequest.StepIds)
                {
                    var stepToRemove = existingRecipe.Steps.SingleOrDefault(step => step.Id == stepId);
                    if (stepToRemove != null)
                    {
                        _context.Remove(stepToRemove);
                    }
                }
            }

            if (deleteRequest.IngredientIds != null && deleteRequest.IngredientIds.Any())
            {
                // Remove specific ingredients
                foreach (var ingredientId in deleteRequest.IngredientIds)
                {
                    var ingredientToRemove = existingRecipe.Ingredients.SingleOrDefault(ingredient => ingredient.Id == ingredientId);
                    if (ingredientToRemove != null)
                    {
                        _context.Remove(ingredientToRemove);
                    }
                }
            }
        }

        await _context.SaveChangesAsync();

        return Ok("Recipe deleted successfully");
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}
    }}