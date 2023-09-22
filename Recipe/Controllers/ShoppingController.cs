using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.DTOs.Request;
using AutoMapper;
using Recipe.DTOs.Request.Common;
using RecipeAPI.DTOs.Response;
using Recipe.DTOs.Response;
using Infrastructure.Repositories.implementation;

namespace Recipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly IBaseRepository<ShoppingItem> shoppingRepository;
        private readonly IMapper _mapper;

        public ShoppingController(IBaseRepository<ShoppingItem> shoppingRepository, IMapper mapper)
        {
            this.shoppingRepository = shoppingRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAllItems/{id}")]
        public async Task<IActionResult> GetAllItems(string id)
        {
            var res = await shoppingRepository.GetAsync(r=>r.UserId == id);

            var result = _mapper.Map<IEnumerable<ResponseShoppingItemDto>>(res);
            return Ok(result);
        }

        [HttpPost("AddShopItem")]
        public IActionResult AddShopItem(ShoppingItemDto itemDto)
        {
            var shoppingItem = _mapper.Map<ShoppingItem>(itemDto);
            shoppingRepository.Add(shoppingItem);
            return Ok(shoppingItem);
        }

        [HttpPut("UpdateShopItem")]
        public IActionResult UpdateShopItem(UpdateShoppingItemDto itemDto)
        {
            var shoppingItem = shoppingRepository.FindByIdAsync(itemDto.Id).Result;
            shoppingItem.isPurchased = itemDto.isPurchased;
            shoppingRepository.Update(shoppingItem);
            return Ok(shoppingItem);
        }

    }
}
