using Notes.Application.Common.Mapping;
using Notes.Application.Notes.Commands.CreateNote;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
namespace Notes.WebAPI.Models
{
    public class CreateNoteDto: IMapWith<CreateNoteCommand>
    {
        [Required] // Нужен для валидации входящих данных и возвращения соответствующих HTTP-ответов, поле ниже становится обязательным
        public string? Title { get; set; }
        public string? Details { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateNoteDto, CreateNoteCommand>().
                ForMember(noteCommand=>noteCommand.Title,opt=>opt.MapFrom(noteDto=>noteDto.Title)).
                ForMember(noteCommand=>noteCommand.Details,opt=>opt.MapFrom(noteDto=>noteDto.Details));
        }
    }
}
