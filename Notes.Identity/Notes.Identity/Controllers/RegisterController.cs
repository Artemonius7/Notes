using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;

namespace Notes.Identity.Controllers
{
    [Route("Register")]
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public RegisterController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
            => (_userManager, _signInManager) = (userManager, signInManager);

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var user = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };

            // Явно указываем путь к представлению, чтобы избежать ошибки ViewNotFound
            return View("~/Views/Register/Register.cshtml", user);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Register/Register.cshtml", viewModel);

            var user = new AppUser
            {
                  UserName = viewModel.Login
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
            {
                // После успешной регистрации лучше всего перенаправить пользователя 
                // обратно на страницу логина (сохранив ReturnUrl), чтобы он зашел штатно через OIDC
                if (!string.IsNullOrEmpty(viewModel.ReturnUrl))
                {
                    // Возвращаем на логин, пробрасывая ReturnUrl дальше
                    return RedirectToAction("Login", "Auth", new { returnUrl = viewModel.ReturnUrl });
                }

                return Redirect("~/");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("~/Views/Register/Register.cshtml", viewModel);
        }
    }
}