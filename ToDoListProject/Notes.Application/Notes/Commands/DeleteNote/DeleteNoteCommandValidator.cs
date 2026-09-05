using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Application.Notes.Commands.DeleteNote
{
    public class DeleteNoteCommandValidator:AbstractValidator<DeleteNoteCommand>
    {
        public DeleteNoteCommandValidator()
        {
            RuleFor(deleteNoteCommand => deleteNoteCommand.UserId).NotEqual(Guid.Empty);
            RuleFor(deleteNoteCommand=> deleteNoteCommand.Id).NotEqual(Guid.Empty);
        }
    }
}
