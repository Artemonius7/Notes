using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MediatR;
using Notes.Domain;
namespace Notes.Application.Notes.Queries.GetNoteList
{
    public class GetNoteListQuery : IRequest<NoteListVm>
    {
        public Guid UserId { get; set; }
    }
}
