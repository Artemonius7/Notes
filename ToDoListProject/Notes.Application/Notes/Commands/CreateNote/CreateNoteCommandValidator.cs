using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
namespace Notes.Application.Notes.Commands.CreateNote
{
    // Class for check of data validation
    public class CreateNoteCommandValidator: AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {
            RuleFor(createNoteCommand =>
            createNoteCommand.Title)
                .NotEmpty()
                .MaximumLength(250);
                
            RuleFor(createMoteCommand =>
            createMoteCommand.UserId).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
