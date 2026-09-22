// Создание модели для валидации логина и пароля
using System.ComponentModel.DataAnnotations;
namespace Notes.Identity.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        public string? Login { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)] // Чтобы при вводе данных пароль не отображался, влияет всего лишь на UI, но никак на безопасность в целом
        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
