using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using Core.Entities;
using Core.Interfaces.Repositories;

using Application.DTOs.Request;
using Application.DTOs.Response;

namespace RecipeApi.Controllers
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
            var res = await shoppingRepository.GetAsync(r => r.UserId == id);

            var result = _mapper.Map<IEnumerable<ResponseShoppingItemDto>>(res);
            return Ok(result);
        }

        [HttpPost("AddShopItem")]
        public IActionResult AddShopItem(ShoppingItemDto itemDto)
        {
            var shoppingItem = _mapper.Map<ShoppingItem>(itemDto);
            shoppingRepository.AddAsync(shoppingItem);
            return Ok(shoppingItem);
        }

        [HttpPut("UpdateShopItem")]
        public IActionResult UpdateShopItem(UpdateShoppingItemDto itemDto)
        {
            
            var shoppingItem = shoppingRepository.GetByIdAsync(itemDto.Id).Result;
            shoppingItem.IsPurchased = itemDto.isPurchased;
            shoppingRepository.Update(shoppingItem);
            return Ok(shoppingItem);
        }

    }
}
