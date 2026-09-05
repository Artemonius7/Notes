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
namespace Notes.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class NoteController : BaseController
    {
        
        private readonly IMapper _mapper;
        public NoteController(IMapper mapper) => _mapper = mapper;

        [HttpGet]
        [Authorize] // Этот эндпоинт доступен только для авторизованных пользователей
        public async Task<ActionResult<NoteListVm>> GetAll() // для получения списка заметок
        {
            var query = new GetNoteListQuery { UserId = UserId };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }
        [HttpGet("{id}")]
        [Authorize]
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
        [HttpPost]
        [Authorize]
        public async Task <ActionResult<Guid>> Create([FromBody] CreateNoteDto createNoteDto) // при помощи FromBody мы берем данные из тела HTTP-запроса
        {
            var command = _mapper.Map<CreateNoteCommand>(createNoteDto);
            command.UserId = UserId;
            var noteId = await Mediator.Send(command);
            return Ok(noteId);
        }
        [HttpPut]
        [Authorize]
        public async Task <IActionResult> Update([FromBody] UpdateNoteDto updateNoteDto)
        {
            var command = _mapper.Map<UpdateNoteCommand>(updateNoteDto);
            command.UserId = UserId;
            var NoteId = await Mediator.Send(command);
            return Ok(NoteId);
        }
        [HttpDelete ("{id}")]
        [Authorize]
        public async Task <IActionResult> Delete(Guid id)
        {
            var query = new DeleteNoteCommand
            {
                UserId = UserId,
                Id = id
            };
            await Mediator.Send(query);
            return NoContent(); 
        }
    }
}
