using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace RecipeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.RegisterAsync(model);
            
            if(!result.Success)
                return BadRequest(result.Message);
            
            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _authService.TokenRequestAsync(model);
            
            if (!result.IsAuthenticated)
                return Unauthorized(result.Message);
            
            return Ok(result);
        }
    }
}
