// Описание стандартной модели пользователя
using Microsoft.AspNetCore.Identity;
namespace Notes.Identity.Models
{
    public class AppUser:IdentityUser
    {
        public string? Login { get; set;  }
        public string? FirstName { get; set; }
        public string? LastName { get; set; } // Знак вопроса допускает значение NULL в переменной
        public DateTime BirthDate { get; set; }
    }
}
