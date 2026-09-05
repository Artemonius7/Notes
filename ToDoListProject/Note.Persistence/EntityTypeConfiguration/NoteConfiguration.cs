using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Notes.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Notes.Persistence.EntityTypeConfiguration
{
    public class NoteConfiguration: IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(note => note.Id);
            builder.HasIndex(note => note.Id).IsUnique();
            builder.Property(note => note.Id).IsRequired();
            builder.Property(node => node.Title).HasMaxLength(250);
            
        }
    }
}
