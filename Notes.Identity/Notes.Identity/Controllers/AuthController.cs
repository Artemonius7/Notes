using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authentication;
using Notes.Identity.Token;
namespace Notes.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly JwtTokenService _jwtTokenService;
        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IIdentityServerInteractionService interactionService,
            JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
            _jwtTokenService = jwtTokenService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Ошибка валидации данных!");

            var user = await _userManager.FindByNameAsync(viewModel.Login);
            if (user == null)
            {
                return BadRequest(new { message = "Пользователь не найден!" });
            }

            var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Неверный логин или пароль!" });
            }
            string token = _jwtTokenService.GenerateToken(user);
            return Ok(new { token = token });
        }
    }
}