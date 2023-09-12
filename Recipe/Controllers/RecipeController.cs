﻿using Microsoft.AspNetCore.Mvc;
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

namespace RecipeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IBaseRepository<Category> categoryRepository;
        private readonly IBaseRepository<Ingredient> ingredientRepository;
        private readonly IBaseRepository<Step> stepRepository;
        private readonly IMapper _mapper;

        public RecipeController(IRecipeRepository recipeRepository
            , IBaseRepository<Ingredient> ingredientRepository
            , IBaseRepository<Step> stepRepository
            , IMapper mapper
            , IBaseRepository<Category> categoryRepository
            )
        {
            this.recipeRepository = recipeRepository;
            this.ingredientRepository = ingredientRepository;
            this.stepRepository = stepRepository;
            _mapper = mapper;
            this.categoryRepository = categoryRepository;
        }
        
        [HttpGet]
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

        [HttpGet("filter")]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeResponse>> GetById(int id)
        {
            var res = await recipeRepository.GetOneById(id);
            
            if (res == null) return NotFound();
            
            return _mapper.Map<RecipeResponse>(res);
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteRecipe(int id)
        {
            var recipe = recipeRepository.GetOneById(id).Result;
            if (recipe == null) return NotFound("Recipe not found");
            recipeRepository.Delete(recipe);
            DeleteIngredientsAndSteps(recipe);
            return Ok(recipe);

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
