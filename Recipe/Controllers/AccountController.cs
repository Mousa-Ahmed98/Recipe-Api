using Application.UserSession;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.CustomModels;
using Infrastructure.Repositories.Implementation;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeAPI.DTOs.Request.Common;
using System.Threading.Tasks;

namespace RecipeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IUserSession _session;
        private readonly IMapper _mapper;

        public AccountController(
            IRecipeRepository recipeRepository, 
            IUserSession session, 
            IMapper mapper
            )
        {
            this.recipeRepository = recipeRepository;
            _session = session;
            _mapper = mapper;
        }


        [HttpGet("my-recipes")]
        public async Task<PaginatedList<RecipeSummary>> GetRecipesByUsername(
            [FromQuery] PaginatedRequest request
            )
        {
            var res = await recipeRepository.GetRecipesByUsername(
                _session.Username,
                request.CurrentPage,
                request.PageSize
                );

            return res;
        }

    }
}
