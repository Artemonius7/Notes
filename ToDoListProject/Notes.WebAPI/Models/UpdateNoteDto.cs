using AutoMapper;
using Notes.Application.Common.Mapping;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.WebAPI.Controllers;

namespace Notes.WebAPI.Models
{
    public class UpdateNoteDto: IMapWith<UpdateNoteCommand>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public void Update (Profile profile)
        {
            profile.CreateMap<UpdateNoteDto, UpdateNoteCommand>().ForMember(noteCommand =>

                noteCommand.Id, opt => opt.MapFrom(noteDto => noteDto.Id)
            ).ForMember(noteCommand => noteCommand.Title, opt => opt.MapFrom(noteDto => noteDto.Title)).
            ForMember(noteCommand => noteCommand.Details, opt => opt.MapFrom(noteDto => noteDto.Details));

            
        }
    }
}
