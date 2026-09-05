using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;
namespace Notes.Identity.Controllers
{
    [Route("Logout")]
    public class LogoutController: Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;
        public LogoutController(SignInManager<AppUser> signInManager, IIdentityServerInteractionService interactionService)
            => (_signInManager,_interactionService) = (signInManager,interactionService);
        [HttpGet]
        public IActionResult Logout(string returnUrl)
        {
            var user = new LogoutViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Logout (string logoutId, CancellationToken cancellationToken)
        {
            // Так как у нас уже есть токен входа, нам нужно его разлогинить для выхода из аккаунта
            await _signInManager.SignOutAsync();
            var logout = await _interactionService.GetLogoutContextAsync(logoutId, cancellationToken);
            return Redirect(logout?.PostLogoutRedirectUri ?? "/" );
        }

    }
}
