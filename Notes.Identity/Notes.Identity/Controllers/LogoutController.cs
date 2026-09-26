using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;
using Notes.Identity.Token;
namespace Notes.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LogoutController: ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        public LogoutController(SignInManager<AppUser> signInManager, IIdentityServerInteractionService interactionService , JwtTokenService jwtTokenService)
            => (_signInManager,_jwtTokenService) = (signInManager,jwtTokenService);
        [HttpPost("logout")]
        public async Task<IActionResult> Logout (string logoutId, CancellationToken cancellationToken)
        {
            // Так как у нас уже есть токен входа, нам нужно его разлогинить для выхода из аккаунта
            await _signInManager.SignOutAsync(); // Удаление куки аутентификации
            return Ok(new { message = "успешный выход из системы" });
        }

    }
}
