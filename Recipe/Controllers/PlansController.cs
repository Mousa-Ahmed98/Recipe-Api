using Application.DTOs.Response;
using Application.UserSession;
using AutoMapper;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlansController : ControllerBase
    {
        private readonly IPlansRepository _plansRepository;
        private readonly IUserSession _session;
        private readonly IMapper _mapper;
        public PlansController(
            IPlansRepository recipeRepository,
            IUserSession session,
            IMapper mapper
            )
        {
            _plansRepository = recipeRepository;
            _session = session;
            _mapper = mapper;
            _plansRepository.SetUserId( _session.UserId );
        }

        [HttpGet]
        public async Task<IActionResult> GetPlans()
        {
            var res = await _plansRepository.GetAllPlans();

            return Ok(
                _mapper.Map<List<PlanResponse>> ( res )
                );
        }

        [HttpPost]
        public async Task<IActionResult> PlanOut(
            [FromQuery] PlanRequest request
            )
        {
            var res = await _plansRepository.PlanOut(
                request.Day, request.RecipeId
                );

            if (res == null) return NotFound();

            return new CreatedResult(nameof(PlanOut), 
                _mapper.Map<PlanResponse>( res )
                );
        }

        [HttpPut("{id}/change-date")]
        public async Task<IActionResult> ChangeDate(
            [FromRoute] int id,
            [FromQuery] string date
        )
        {
            var res = await _plansRepository.ChangePlanDate(id, date);

            if (res == false) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> PlanOff(
            [FromRoute] int id
            )
        {
            var res = await _plansRepository.PlanOff(id);

            if (res == false) return NotFound();

            return NoContent();
        }

    }
}
