#region class Entities.Note
#pragma warning disable IDE0130, IDE0240
#nullable enable

namespace NullPointersEtc.NotesJournalApp.NoteEntity;

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

#region interface INoteRepository
public interface INoteRepository
{
    System.Threading.Tasks.Task<Note> CreateAsync(Note note);

    System.Threading.Tasks.Task<Note> GetAsync(Guid id);

    System.Threading.Tasks.Task<
        System.Collections.Generic.IEnumerable<Note>>
        SearchAsync(string query);

    System.Threading.Tasks.Task<Note> UpdateAsync(Note note);

    System.Threading.Tasks.Task DeleteAsync(Guid id);
}
#endregion interface INoteRepository


#region Exception classes
public class NoteIdIsNotSetException : System.InvalidOperationException
{
    public override string Message { get => "Note Id must be set first"; }
}


public class NoteTitleIsNotSetException : System.InvalidOperationException
{
    public override string Message { get => "Note Title must be set first"; }
}
#endregion class Exceptions.NoteTitleIsNotSetException

#region class Exceptions.NoteTitleIsEmptyException
public class NoteTitleIsEmptyException : System.ArgumentException
{
    public override string Message { get => "Note Title must not be empty"; }
}
#endregion class Exceptions.NoteTitleIsEmptyException

#region class Exceptions.NoteBodyIsNotSetException
public class NoteBodyIsNotSetException : System.InvalidOperationException
{
    public override string Message { get => "Note Body must be set first"; }
}
#endregion class Exceptions.NoteBodyIsNotSetException

#region class Exceptions.NoteBodyIsEmptyException
public class NoteBodyIsEmptyException : System.ArgumentException
{
    public override string Message { get => "Note Body must not be empty"; }
}
#endregion class Exceptions.NoteBodyIsEmptyException

#region class Exceptions.NoteUpdatedAtIsNotSetException
public class NoteUpdatedAtIsNotSetException : System.InvalidOperationException
{
    public override string Message { get => "Note UpdatedAt must be set first"; }
}
#endregion class Exceptions.NoteUpdatedAtIsNotSetException

#region class Exceptions.NoteCreatedAtIsNotSetException
public class NoteCreatedAtIsNotSetException : System.InvalidOperationException
{
    public override string Message { get => "Note CreatedAt must be set first"; }
}
#endregion class Exceptions.NoteCreatedAtIsNotSetException
