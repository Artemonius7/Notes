using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes.Queries.GetNoteList;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Application.Notes.Commands.CreateNote;
using Notes.WebAPI.Models;
using System.Reflection;
using MediatR;
using AutoMapper;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Application.Notes.Commands.DeleteNote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Notes.WebAPI.Controllers
{
    [Produces("application/json")] // Контроллеры возвращают ответы в формате JSON-документа
    [Route("api/[controller]")]
    public class NoteController : BaseController
    {
        // summary необходим для описания нашего метода и для добавления комментария в XML-Документ для работы со Swagger'ом
        // В remarks можно добавить как сведения в формате текста или JSON-документа, XML, что предоставляет более надежный интерфейс в Swagger
        private readonly IMapper _mapper;
        public NoteController(IMapper mapper) => _mapper = mapper;
        /// <summary> 
        /// Получить список заметок (Gets the list of your Notes)
        /// </summary>
        /// <remarks>Пример запроса (Sample Request):        
        /// GET /api/note</remarks>
        /// <returns>Возвращает объект NoteListVm (представление) со списком заметок пользователя</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unathorized</response>
        /// 
        [HttpGet]
        [Authorize] // Этот эндпоинт доступен только для авторизованных пользователей
        [ProducesResponseType(typeof(NoteListVm),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<NoteListVm>> GetAll() // для получения списка заметок
        {
            var query = new GetNoteListQuery { UserId = UserId };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        // param - это описание для входящих параметров и их дальнейшей валидации
        /// <summary>
        /// Получить определенную заметку по ее идентификатору
        /// </summary>
        /// <param name="id">Note Id (Guid)</param>
        /// <remarks>Пример запроса (Sample Request):
        /// GET /api/note/{38B7CB0E-D29F-46A0-9D8A-89A4CD1AAF79}</remarks>
        /// <returns>Returns NoteDetailsVm</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(NoteDetailsVm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoteDetailsVm>> Get(Guid id)
        {
            var query = new GetNoteDetailsQuery
            {
                UserId = UserId,
                Id = id
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        /// <summary>
        /// Создать новую заметку (Create the note)
        /// </summary>
        /// <param name="createNoteDto">Name of Note</param>
        /// <remarks>Пример запроса (Sample Request):
        /// POST /api/note
        /// {
        ///     title:"Название заметки",
        ///     details:"Описание заметки"
        /// }</remarks>
        /// <returns>Return Guid</returns>
        /// <response code="201">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task <ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto) // при помощи FromBody мы берем данные из тела HTTP-запроса
        {
            var command = _mapper.Map<CreateNoteCommand>(createNoteDto);
            command.UserId = UserId;
            var noteId = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = noteId }, noteId);
        }
        /// <summary>
        /// Редактировать заметку (Edit your Note)
        /// </summary>
        /// <param name="updateNoteDto">New Name of Note</param>
        /// <remarks>Пример запроса (Sample Request):
        /// PUT /api/note
        /// {
        ///     "id":"{38B7CB0E-D29F-46A0-9D8A-89A4CD1AAF79}",
        ///     "title":"Название заметки",
        ///     "details":"Описание заметки"
        /// }
        /// "</remarks>
        /// <returns>Returns IActionResult</returns>
        /// <response code="200">Success</response>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
        {
            var command = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
            command.UserId = UserId;
            var isUpdated = await Mediator.Send(command);
            return Ok(isUpdated);
        }
        /// <summary>
        /// Удалить заметку (Delete the Note)
        /// </summary>
        /// <param name="id">Note ID (Guid)</param>
        /// <remarks>Пример запроса (Sample Request):
        /// DELETE /api/note/{38B7CB0E-D29F-46A0-9D8A-89A4CD1AAF79}</remarks>
        /// <returns>Returns IActionResult</returns>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not Found</response>
        [HttpDelete ("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <IActionResult> Delete(Guid id)
        {
            var command = new DeleteNoteCommand
            {
                UserId = UserId,
                Id = id
            };
            await Mediator.Send(command);
            return NoContent(); 
        }
    }
}
