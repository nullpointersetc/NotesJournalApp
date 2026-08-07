#region "NotesDomain/NoteEntity.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240
#nullable enable

using DateTime = System.DateTime;
using Guid = System.Guid;
using Key = System.ComponentModel.DataAnnotations.KeyAttribute;
using StringLength = System.ComponentModel.DataAnnotations.StringLengthAttribute;
using InvalidOperationException = System.InvalidOperationException;
using ArgumentException = System.ArgumentException;

using TaskReturningNote = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.NoteEntity.Note>;

using TaskReturningNotes = System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.NoteEntity.Note>>;

using Task = System.Threading.Tasks.Task;

namespace NullPointersEtc.NotesJournalApp.NoteEntity;

public sealed class Note
{
    [property: Key]
    public Guid NoteID
    {
        get => noteIdField ?? throw new NoteIdIsNotSetException();
        set => noteIdField = value;
    }

    [property: StringLength(
        maximumLength: MAX_TITLE_LENGTH,
        MinimumLength = 1)]
    public string Title
    {
        get => titleField ?? throw new NoteTitleIsNotSetException();

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new NoteTitleIsEmptyException();
            else if (value.Length > MAX_TITLE_LENGTH)
                throw new NoteTitleIsTooLongException();
            else
                titleField = value;
        }
    }

    [property: StringLength(
        maximumLength: MAX_BODY_LENGTH,
        MinimumLength = 1)]
    public string Body
    {
        get => bodyField ?? throw new NoteBodyIsNotSetException();

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new NoteBodyIsEmptyException();
            else if (value.Length > MAX_BODY_LENGTH)
                throw new NoteBodyIsTooLongException();
            else
                bodyField = value;
        }
    }

    public DateTime CreatedAt
    {
        get => createdAtField ??
            throw new NoteCreatedAtIsNotSetException();

        set
        {
            if (createdAtField is null)
                createdAtField = value;
            else if (createdAtField == value)
                return;
            else
                throw new NoteCreatedAtWouldBeChangedException();
        }
    }

    public DateTime LastModifiedAt
    {
        get => lastModifiedAtField ??
            throw new NoteLastModifiedAtIsNotSetException();

        set
        {
            if (lastModifiedAtField is null)
                lastModifiedAtField = value;
            else if (lastModifiedAtField < value)
                lastModifiedAtField = value;
            else if (LastModifiedAt > value)
                throw new NoteLastModifiedDateWouldGoBackInTimeException();
        }
    }

    private Guid? noteIdField = null;
    private string? titleField = null;
    private string? bodyField = null;
    private DateTime? createdAtField = null;
    private DateTime? lastModifiedAtField = null;

    public const int MAX_TITLE_LENGTH = 250;
    public const int MAX_BODY_LENGTH = 4000;
}


public interface INoteRepository
{
    TaskReturningNote CreateNoteAsync(Note note);

    TaskReturningNotes GetAllNotesAsync();

    TaskReturningNote GetNoteByIdAsync(Guid noteID);

    TaskReturningNotes SearchNotesAsync(string query);

    TaskReturningNote UpdateNoteAsync(Note note);

    Task DeleteNoteAsync(Guid noteID);
}


public sealed class NoteIdIsNotSetException
    : InvalidOperationException
{
    public override string Message { get => "NoteID must be set first"; }
}


public sealed class NoteTitleIsNotSetException
    : InvalidOperationException
{
    public override string Message { get => "Note Title must be set first"; }
}


public sealed class NoteTitleIsEmptyException
    : ArgumentException
{
    public override string Message { get => "Note Title must not be empty"; }
}


public sealed class NoteTitleIsTooLongException
    : ArgumentException
{
    public override string Message
    {
        get => "Note Title must be " +
            Note.MAX_TITLE_LENGTH + " characters or shorter";
    }
}


public sealed class NoteBodyIsNotSetException
    : InvalidOperationException
{
    public override string Message { get => "Note Body must be set first"; }
}


public sealed class NoteBodyIsEmptyException
    : ArgumentException
{
    public override string Message { get => "Note Body must not be empty"; }
}


public sealed class NoteBodyIsTooLongException
    : ArgumentException
{
    public override string Message
    {
        get => "Note Body must be " +
            Note.MAX_BODY_LENGTH + " characters or shorter";
    }
}


public sealed class NoteCreatedAtIsNotSetException
    : InvalidOperationException
{
    public override string Message
    {
        get => "Note CreatedAt must be set first";
    }
}


public sealed class NoteCreatedAtWouldBeChangedException
    : InvalidOperationException
{
    public override string Message
    {
        get => "Note CreatedAt cannot be changed. " +
            "This is a data-integrity issue";
    }
}


public class NoteLastModifiedAtIsNotSetException
    : InvalidOperationException
{
    public override string Message
    {
        get => "Note LastModifiedAt must be set first";
    }
}


public class NoteLastModifiedDateWouldGoBackInTimeException
    : InvalidOperationException
{
    public override string Message
    {
        get => "Note LastModifiedAt cannot be set to an earlier time. " +
            "This is a data-integrity issue";
    }
}
#endregion "NotesDomain/NoteEntity.cs"
