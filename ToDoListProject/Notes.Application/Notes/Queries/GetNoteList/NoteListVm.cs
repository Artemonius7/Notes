using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MediatR;
using Notes.Application.Interfaces;
namespace Notes.Application.Notes.Queries.GetNoteList
{
    public class NoteListVm
    {
        public IList<NoteLookupDto> Notes { get; set; }

    }
}
