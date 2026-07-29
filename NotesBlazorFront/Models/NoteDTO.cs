﻿#region "NoteDto.cs"
namespace NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models;
public record NoteDTO(
    System.Guid Id,
    string Title, string Body,
    System.DateTime CreatedAt,
    System.DateTime UpdatedAt);

#endregion "NoteDto.cs"
