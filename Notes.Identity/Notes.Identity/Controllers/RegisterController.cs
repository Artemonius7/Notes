using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;
namespace Notes.Identity.Controllers
{
    [Route("Register")]
    public class RegisterController: Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public RegisterController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
            => (_userManager, _signInManager) = (userManager, signInManager);
        [HttpGet]
        public IActionResult Register (string returnUrl)
        {
            var user = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Register (RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            if (viewModel.Password!=viewModel.ConfirmPassword)
            {
                ModelState.AddModelError("", "Пароли не совпадают!");
                return View(viewModel);
            }
            var user = new AppUser
            {
                Login = viewModel.Login
            };
            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user,false);
                return Redirect(viewModel.ReturnUrl ?? "/");
            }
            ModelState.AddModelError("", "Произошла ошибка при регистрации");
            return View(viewModel);
        }
    }
}
