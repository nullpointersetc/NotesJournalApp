#region class Entities.Note
#pragma warning disable IDE0240
#nullable enable

using NoteIdIsNotSetException =
    NullPointersEtc.NotesJournalApp.Exceptions.NoteIdIsNotSetException;

using NoteTitleIsNotSetException =
    NullPointersEtc.NotesJournalApp.Exceptions.NoteTitleIsNotSetException;

using NoteTitleIsEmptyException =
    NullPointersEtc.NotesJournalApp.Exceptions.NoteTitleIsEmptyException;

using NoteBodyIsNotSetException =
    NullPointersEtc.NotesJournalApp.Exceptions.NoteBodyIsNotSetException;

using NoteCreatedAtIsNotSetException =
    NullPointersEtc.NotesJournalApp.Exceptions.NoteCreatedAtIsNotSetException;

using NoteUpdatedAtIsNotSetException =
    NullPointersEtc.NotesJournalApp.Exceptions.NoteUpdatedAtIsNotSetException;

namespace NullPointersEtc.NotesJournalApp.Entities;

public class Note
{
    public Guid Id
    {
        get => myGuid ?? throw new NoteIdIsNotSetException();
        set => myGuid = value;
    }

    public string Title
    {
        get => myTitle ?? throw new NoteTitleIsNotSetException();
        set => myTitle = string.IsNullOrWhiteSpace(value)
                ? throw new NoteTitleIsEmptyException()
                : value;
    }

    public string Body
    {
        get => myBody ?? throw new NoteBodyIsNotSetException();
        set => myBody = value;
    }

    public DateTime CreatedAt
    {
        get => myCreatedAt ?? throw new NoteCreatedAtIsNotSetException();
        set => myCreatedAt = value;
    }

    public DateTime UpdatedAt
    {
        get => myUpdatedAt ?? throw new NoteUpdatedAtIsNotSetException();
        set => myUpdatedAt = value;
    }

    private System.Guid? myGuid = null;
    private string? myTitle = null;
    private string? myBody = null;
    private System.DateTime? myCreatedAt = null;
    private System.DateTime? myUpdatedAt = null;
}
#endregion class Entities.Note
