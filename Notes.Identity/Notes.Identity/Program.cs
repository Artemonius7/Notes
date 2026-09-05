using System.IO;
using Microsoft.Extensions.FileProviders;
using Notes.Identity;
using Notes.Identity.Data;
using Notes.Identity.Models;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.DependencyInjection;
using Duende.IdentityServer.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using Microsoft.Extensions.FileProviders;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DBConnection");
// Добавляем контекст базы данных
// Можно писать как options, так и config : это обычная переменная, которая отличается лишь названием и не несет системной смысловой нагрузки
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlite(connectionString);
});
builder.Services.AddIdentity<AppUser, IdentityRole>(config =>
{
    // Подключаем Microsoft.AspNetCore.Identity и Notes.Identity.Models
    // Настраиваем конфигурацию для пароля 
    config.Password.RequiredLength = 4;
    config.Password.RequireDigit = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders(); // Для обновления и получения токенов доступа

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<AppUser>()
    .AddInMemoryApiResources(Configuration.ApiResources)
    .AddInMemoryApiScopes(Configuration.ApiScopes)
    .AddInMemoryIdentityResources(Configuration.IdentityResources)
    .AddInMemoryClients(Configuration.Clients)
    .AddDeveloperSigningCredential()
    .AddTestUsers(Configuration.Users.ToList());
builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "Notes.Identity.Server";
    config.LoginPath = "/Auth/Login";
    config.LogoutPath = "/Auth/Logout";
});
builder.Services.AddControllersWithViews(); // Добавление контроллеров и представлений для нашего приложения
var app = builder.Build();

// Проверка на существование контекста базы данных в рамках запроса?
// Scope - Это как один рабочий сеанс, в рамках которого мы будем проверять наш контекст базы данных
// Using - гарантия того, что ресурсы внутри этого блока будут очищены после завершения сеанса с контекстом базы данных при получении http-запроса от пользователя
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var context = serviceProvider.GetRequiredService<AuthDbContext>();
        DbInitializer.Initialize(context);
    }
    // Обработка исключения и запись в логи
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка при инициализации базы данных!");
    }

}
app.UseRouting(); // ИСпользовать маршрутизацию контроллеров
app.UseIdentityServer();
app.UseEndpoints(endpoint =>
{
    endpoint.MapDefaultControllerRoute(); // Маппинг роутинга по имени контроллеров
});

// Делаем раздачу статических стилей из другой директории
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath,"Styles")), // Указание физической папки на диске, куда будут сохраняться стили, а не в дефолтную wwwroot
    RequestPath = "/styles" // Путь к запросу по ссылке на styles
});
app.Run();
