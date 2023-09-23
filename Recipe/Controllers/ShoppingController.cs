using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            shoppingRepository.Add(shoppingItem);
            return Ok(shoppingItem);
        }

        [HttpPut("UpdateShopItem")]
        public IActionResult UpdateShopItem(UpdateShoppingItemDto itemDto)
        {
            
            var shoppingItem = shoppingRepository.GetById(itemDto.Id).Result;
            shoppingItem.isPurchased = itemDto.isPurchased;
            shoppingRepository.Update(shoppingItem);
            return Ok(shoppingItem);
        }

    }
}
