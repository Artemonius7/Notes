using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Domain
{
    public class Note
    {
        public Guid Id { get; set; } // ID of Note
        public Guid UserId { get; set; } // ID of User

        public string Title {  get; set; } // Title of the note
        public string Details {  get; set; } // Description of the details of the note

        public DateTime CreationDate { get; set; } // Data of creation of the note
        public DateTime? EditDate { get; set; } // When the note was edit
    }
}
