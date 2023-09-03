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
    }
}
