using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Duende.IdentityServer.Services;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Notes.Identity.Models;
using Notes.Identity.Token;

namespace Notes.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly JwtTokenService _jwtTokenService;
        public RegisterController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IIdentityServerInteractionService interaction,
            JwtTokenService jwtTokenService)
            => (_userManager, _signInManager, _interaction,_jwtTokenService) = (userManager, signInManager, interaction,jwtTokenService);

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel viewModel)
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
                string token = _jwtTokenService.GenerateToken(user);
                return Ok(new { token = token });
            }
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            return BadRequest(new { message = errors });
        }
    }
}