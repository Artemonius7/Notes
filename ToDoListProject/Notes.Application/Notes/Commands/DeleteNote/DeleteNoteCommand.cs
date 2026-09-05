using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Notes.Domain;
using MediatR;
namespace Notes.Application.Notes.Commands.DeleteNote
{
    // Все, что нужно для удаления заметки
    public class DeleteNoteCommand: IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}
