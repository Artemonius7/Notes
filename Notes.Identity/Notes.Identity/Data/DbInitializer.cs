// Первичная инициализация базы данных при запуске приложения
using Microsoft.EntityFrameworkCore;

namespace Notes.Identity.Data
{
    public class DbInitializer
    {
        public static void Initialize(AuthDbContext context)
        {
            context.Database.Migrate();
        }
    }
}
