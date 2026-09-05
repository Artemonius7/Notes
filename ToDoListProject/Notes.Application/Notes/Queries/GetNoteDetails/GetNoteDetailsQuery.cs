using System;
using System.Collections.Generic;
using System.Text;
using Notes.Domain;
using Notes.Application;
using MediatR;
namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    // Класс для запроса вывода списка вех заметок пользователя
    public class GetNoteDetailsQuery : IRequest<NoteDetailsVm>
    {
        public Guid UserId { get; set; }
        public Guid Id {  get; set; } 
    }
}
