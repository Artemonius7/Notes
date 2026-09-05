using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
namespace Notes.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandValidator: AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteCommandValidator() 
        {
            RuleFor(updateNoteCommand => updateNoteCommand.UserId).NotEqual(Guid.Empty);
            RuleFor(updateNoteCommand => updateNoteCommand.Id).NotEqual(Guid.Empty);
            RuleFor(updateNoteCommmand => updateNoteCommmand.Title).MaximumLength(250).NotEmpty();

        }
    }
}
