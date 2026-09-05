using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Notes.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")] // маршрут для контроллеров и действий
    public abstract class BaseController: ControllerBase

    {
        public IMediator _mediator; 
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>(); // для формирования команд при выполнении запросов
        internal Guid UserId => !User.Identity.IsAuthenticated ? Guid.Empty : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); // получение ид пользователя
    }
}
