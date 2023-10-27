using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Application.DTOs.Response;
using Application.Interfaces.DomainServices;

namespace RecipeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        public CategoryController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAllCats()
        {
            return Ok(
                await _categoriesService.GetAllCategories()
                );
        }
    }
}
