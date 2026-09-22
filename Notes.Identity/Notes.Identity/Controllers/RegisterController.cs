using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Duende.IdentityServer.Services;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Notes.Identity.Models;

namespace Notes.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;

        public RegisterController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IIdentityServerInteractionService interaction)
            => (_userManager, _signInManager, _interaction) = (userManager, signInManager, interaction);

        [HttpGet("register")]
        public IActionResult Register(string returnUrl)
        {
            var user = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };
            string token = "token"; // работает как заглушка для проверки данных от регистрации до сохранения в LocalStorage
            return Ok(new { token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Невалидные данные" });
            }

            var user = new AppUser
            {
                UserName = viewModel.Login
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
            {
                string token = "token";
                return Ok(new { token = token });
            }
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            return BadRequest(new { message = errors });
        }
    }
}