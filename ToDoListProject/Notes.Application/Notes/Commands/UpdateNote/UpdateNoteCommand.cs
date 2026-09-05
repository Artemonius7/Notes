using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
namespace Notes.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommand: MediatR.IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
    }
}
