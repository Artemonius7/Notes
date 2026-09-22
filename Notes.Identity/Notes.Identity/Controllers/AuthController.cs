using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Duende.IdentityServer.Services;

namespace Notes.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
        }
        [HttpGet("login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                var context = await _interactionService.GetAuthorizationContextAsync(returnUrl, HttpContext.RequestAborted);
                if (context != null || _interactionService.IsValidReturnUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return Redirect("http://localhost:3000");
            }

            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ReturnUrl))
            {
                viewModel.ReturnUrl = HttpContext.Request.Query["returnUrl"];
            }

            if (!ModelState.IsValid)
                return View(viewModel);

            var user = await _userManager.FindByNameAsync(viewModel.Login);
            if (user == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(viewModel);
            }

            if (!string.IsNullOrEmpty(viewModel.ReturnUrl))
            {
                var context = await _interactionService.GetAuthorizationContextAsync(viewModel.ReturnUrl, HttpContext.RequestAborted);
                if (context != null || _interactionService.IsValidReturnUrl(viewModel.ReturnUrl))
                {
                    return Redirect(viewModel.ReturnUrl);
                }
            }

            return Redirect("http://localhost:3000");
        }
    }
}