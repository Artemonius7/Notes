// Первичная инициализация базы данных при запуске приложения
namespace Notes.Identity.Data
{
    public class DbInitializer
    {
        public static void Initialize(AuthDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
