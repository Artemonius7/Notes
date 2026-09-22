using System;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using System.Collections.Generic;
using Notes.Identity.Models;
namespace Notes.Identity
{
    public static class IdentityConfiguration
    {
        // Хранение данных в памяти для клиентов ресурсов
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                // Стандартные ресурсы Open ID Connect
                .AddInMemoryIdentityResources(new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                })
                // Области доступа для API
                .AddInMemoryApiScopes(new List<ApiScope>
                {
                    new ApiScope("NotesWebAPI","Доступ к WebAPI заметок")
                })
                // Регистрация клиентской части приложения (фронтенд)
                .AddInMemoryClients(new List<Client>
                {
                    new Client
                    {
                        ClientId="notes-web-api",
                        ClientName="NotesWebAPI",
                        AllowedGrantTypes = GrantTypes.Code,
                        RequirePkce = true,
                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "NotesWebAPI"
                        },
                        RedirectUris = {"http://localhost:3000/signin-oidc"},
                        PostLogoutRedirectUris = { "http://localhost:3000/signout-oidc"},
                        AllowOfflineAccess = true // использование Refresh Token

                    }
                })
                // Временный ключ шифрования токенов для локальной разработки
                .AddDeveloperSigningCredential();
            return services;
        }
    }
}
