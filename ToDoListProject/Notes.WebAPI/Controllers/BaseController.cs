using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Notes.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")] // маршрут для контроллеров и действий
    public abstract class BaseController: ControllerBase

    {
        public IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>(); // для формирования команд при выполнении запросов
        // Безопасное получение ID текущего пользователя из JWT-токена
        internal Guid UserId
        {
            get
            {
                if (User.Identity?.IsAuthenticated != true)
                    return Guid.Empty;

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
            }
        }
    }
}
