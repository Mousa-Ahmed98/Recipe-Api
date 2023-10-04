using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Infrastructure.Exceptions;
using Infrastructure.Repositories.Interfaces;

using Application.DTOs.Response;
using Application.UserSession;


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
            try
            {
                var res = await _plansRepository.PlanOut(
                    request.Day, request.RecipeId
                );

                return new CreatedResult(nameof(PlanOut),
                    _mapper.Map<PlanResponse>(res)
                    );
            }
            catch (UnAuthorizedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}/change-date")]
        public async Task<IActionResult> ChangeDate(
            [FromRoute] int id,
            [FromQuery] string date
        )
        {
            try 
            { 
                var res = await _plansRepository.ChangePlanDate(id, date);

                return NoContent();
            }
            catch (UnAuthorizedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> PlanOff(
            [FromRoute] int id
            )
        {
            try 
            { 
                var res = await _plansRepository.PlanOff(id);

                return NoContent();
            }
            catch (UnAuthorizedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
