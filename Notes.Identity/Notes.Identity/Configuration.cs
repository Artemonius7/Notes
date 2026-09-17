
// Описание базовой конфигурации сервера авторизации
using Duende.IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.DependencyInjection;
using Duende.IdentityServer.Test;
using System.Linq;
using System.Runtime.CompilerServices;
using Notes.Identity.Data;
using Microsoft.EntityFrameworkCore;
namespace Notes.Identity;
public static class IdentityServerServiceCollectionExtensions
{
    public static IServiceCollection AddNotesIdentityServer (this IServiceCollection services)
    {
        
        
        // Регистрация базовых сервисов IdentityServer
        services.AddIdentityServer()
            .AddInMemoryApiResources(Configuration.ApiResources) // Регистрация в оперативной памяти списка защищаемых веб-API
            .AddInMemoryIdentityResources(Configuration.IdentityResources) // Регистрация identity-ресурсов, которые отвечают за хранение информации о пользователе 
            .AddInMemoryClients(Configuration.Clients) // Регистрация списка прил-ий (клиентов), которым разрешен доступ к IdentityServer
            .AddInMemoryApiScopes(Configuration.ApiScopes) // Регистрация областей доступа (прав доступа) для API. Они дают разрешение на выполнение определенных действий, связанных с IdentityServer
            .AddDeveloperSigningCredential() // Генерирует ключи подписи в памяти для проверки подлинности JWT-токена
            .AddTestUsers(Configuration.Users.ToList());
            return services;
    }

}
public class Configuration
{
    // для поключения к базе данных по ссылке из appsettings.json, получение строки подключения
    public IConfiguration AppConfiguration { get; } // Интерфейс для работы с настройками
    public Configuration(IConfiguration configuration) => AppConfiguration = configuration;
    public string connectionString => AppConfiguration.GetValue<string>("DBConnection");

    // Описание списка защищенных API-сервисов, которые могут получить доступ к защищенным ресурсам
    public static IEnumerable<ApiResource> ApiResources =>
       new List<ApiResource>
       {
           new ApiResource("NotesWebAPI","Access To NotesAPI", new [] {JwtClaimTypes.Name}) // в токен доступа можно включать имя пользователя
           {
               Scopes = {"NotesWebAPI"}
           }
       };

    // Регистрация и получение сведений о клиенте, которые можно запросить
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    // Описание конкретных прав доступа, которые может запрашивать клиент
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("NotesWebAPI","WebAPI")
        };
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "notes-web-api",
                ClientName = "Notes Web",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false, // Пароль клиента не нужен
                RequirePkce = true,
                RedirectUris =
                {
                    "http://localhost:3000/signin-oidc" // перенаправление после аутентификации клиентского приложения
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:3000"
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:3000/signout-oidc" // перенаправление после выхода из авторизованного режима клиента
                },
                AllowedScopes = // Доступные области для нашего клиента, чтобы мы могли с фронтенда получить доступ к IdentityServer
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "NotesWebAPI"
                },
                AllowAccessTokensViaBrowser = true

            }
        };
    public static IEnumerable<TestUser> Users =>
        new List<TestUser>
        { 
            new TestUser
            {
                SubjectId = "1",
                Username = "Artem",
                Password = "qwerty"
            }
        };

} 