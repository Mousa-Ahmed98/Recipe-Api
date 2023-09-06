using AutoMapper;
using Core.Entities;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreEntities = Core.Entities;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IBaseRepository<Category> categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(IBaseRepository<Category> categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCats()
        {
            IEnumerable<Category> categories = await categoryRepository.GetAsync();
            return Ok(categories);
        }
    }
}
