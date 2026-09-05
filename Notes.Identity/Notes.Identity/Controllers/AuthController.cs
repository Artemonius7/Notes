using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Duende.IdentityServer.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding; // Алиас для создания переменной для указания маршрута для контроллера, используя библиотеку
namespace Notes.Identity.Controllers
{
    [Route("Auth")]
    public class AuthController:Controller // Контроллер для аутентификации
    {
        // private readonly отвечает за хранение ссылок внутри контроллера, чтобы их использовать в любом методе класса
        private readonly UserManager<AppUser> _userManager; // нужен дял управления пользователями и их данных
        private readonly SignInManager<AppUser> _signInManager; // отвечает за аутентификацию пользователя и его права доступа
        private readonly IIdentityServerInteractionService _interactionService; // Отвечает за логаут пользователя
        // Конструктор для инициализации пользователя
        public AuthController (UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IIdentityServerInteractionService interactionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
        }
        [HttpGet("Login")]
        public IActionResult Login(string returnUrl) // открытие страницы для ввода логина и пароля ( пустая форма)
        {
            var viewModel = new LoginViewModel // LoginViewModel - это класс, который используется для упаковки данных между контроллером и представлением 
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel); // Возвращает представление (HTML-страницу) с формой логина
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel viewModel) // срабатывает, когда пользователь отправляет форму (после ввода данных и нажатия кнопки "Войти"
        {
            if (!ModelState.IsValid) // ModelState знает о состоянии View-модели
                return View(viewModel); // если данные не прошли валидацию

            var user = await _userManager.FindByNameAsync(viewModel.Login); // Отвечает за CRUD пользователей
            if (user==null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль"); // Ошибка о состоянии модели
                return View(viewModel);
            }
            // SignManager отвечает за вход пользователя и его право доступа
            var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, isPersistent: false, lockoutOnFailure: false); //isPersistent связан с сохранением последующей аутентификации, Lockout - для блокировки аккаунта в случае нескольких неудачных попыток
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(viewModel);
            }
            return Redirect(viewModel.ReturnUrl ?? "/"); // Перенаправление браузера на следующий URL
        }
    }
}
