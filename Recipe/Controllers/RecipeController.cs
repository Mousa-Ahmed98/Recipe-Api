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

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IBaseRepository<CoreEntities.Recipe> recipeRepository;
        private readonly IMapper _mapper;

        public RecipeController(IBaseRepository<CoreEntities.Recipe> recipeRepository, IMapper mapper)
        {
            this.recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        [HttpPost("Add")]
        public IActionResult AddRecipe([FromBody] CreateRecipeRequest recipeDto)
        {

            try
            {
                string validationMessage = recipeDto.Validata();
                if (validationMessage == "")
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


        //[HttpPost("Add")]
        //public IActionResult AddRecipe([FromBody] CreateRecipeRequest recipeDto)
        //{
        //    try
        //    {
        //        // Validate the request
        //        string validationMessage = recipeDto.Validata();
        //        if (!string.IsNullOrEmpty(validationMessage))
        //        {
        //            return BadRequest(new { error = validationMessage });
        //        }

        //        // Mapping and adding the recipe
        //        var recipe = _mapper.Map<CoreEntities.Recipe>(recipeDto);
        //        recipeRepository.Add(recipe);

        //        // Return a 201 Created response with the newly created recipe
        //        return CreatedAtAction(nameof(recipe), new { id = recipe.Id }, recipe);
        //    }
        //    catch (Exception e)
        //    {
        //        // Handle unexpected errors and return a 500 Internal Server Error
        //        return StatusCode(500, new { error = "An error occurred while processing your request." });
        //    }
        //}














        [HttpPut("Edit")]
        public async Task<IActionResult> EditRecipe([FromBody] UpdateRecipeRequest updateRecipeDto)
        {
            try
            {
                // get Recipe by ID
                var existingRecipe = await recipeRepository.GetById(updateRecipeDto.Id);
                if (existingRecipe == null)
                {
                    return NotFound("Recipe not found.");
                }

                // Update the recipe properties
                existingRecipe.Name = updateRecipeDto.Name;
                existingRecipe.Image = updateRecipeDto.Image;
                //existingRecipe.CategoryId = updateRecipeDto.CategoryId;
                // You should implement a method to update ingredients and steps based on your logic.

                // Save the changes to the database
                await recipeRepository.Update(existingRecipe);

                return Ok(existingRecipe);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            try
            {
                // Retrieve the existing recipe by its ID
                var existingRecipe = await recipeRepository.GetById(id);
                if (existingRecipe == null)
                {
                    return NotFound("Recipe not found.");
                }

                // You can add additional logic for authorization or validation here if needed.

                // Delete the recipe
                await recipeRepository.Delete(existingRecipe);

                return Ok("Recipe deleted successfully.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
    
