using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;
namespace Notes.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LogoutController: ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;
        public LogoutController(SignInManager<AppUser> signInManager, IIdentityServerInteractionService interactionService)
            => (_signInManager,_interactionService) = (signInManager,interactionService);
        [HttpGet("logout")]
        public IActionResult Logout(string returnUrl)
        {
            var user = new LogoutViewModel
            {
                ReturnUrl = returnUrl
            };
            string token = "token";
            return Ok(new { token = token });
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout (string logoutId, CancellationToken cancellationToken)
        {
            // Так как у нас уже есть токен входа, нам нужно его разлогинить для выхода из аккаунта
            await _signInManager.SignOutAsync();
            return Ok(new { message = "успешный выход из системы" });
        }

    }
}
