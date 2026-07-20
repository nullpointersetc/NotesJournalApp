#region class Exceptions.NotesException
#pragma warning disable IDE0240
#pragma warning disable IDE0290
#nullable enable

namespace NullPointersEtc.NotesJournalApp.Exceptions;

public class NotesException : System.Exception
{
    public override string Message { get => "Undefined error"; }
}
#endregion class Exceptions.NotesException

#region class Exceptions.NoteIdIsNotSetException
public class NoteIdIsNotSetException : NotesException
{
    public override string Message { get => "Note Id must be set first"; }
}
#endregion class Exceptions.NoteIdIsNotSetException

#region class Exceptions.NoteTitleIsNotSetException

public class NoteTitleIsNotSetException : NotesException
{
    public override string Message { get => "Note Title must be set first"; }
}
#endregion class Exceptions.NoteTitleIsNotSetException

#region class Exceptions.NoteTitleIsEmptyException
public class NoteTitleIsEmptyException : NotesException
{
    public override string Message { get => "Note Title must not be empty"; }
}
#endregion class Exceptions.NoteTitleIsEmptyException

#region class Exceptions.NoteBodyIsNotSetException
public class NoteBodyIsNotSetException : NotesException
{
    public override string Message { get => "Note Body must be set first"; }
}
#endregion class Exceptions.NoteBodyIsNotSetException

#region class Exceptions.NoteBodyIsEmptyException
public class NoteBodyIsEmptyException : NotesException
{
    public override string Message { get => "Note Body must not be empty"; }
}
#endregion class Exceptions.NoteBodyIsEmptyException

#region class Exceptions.NoteUpdatedAtIsNotSetException
public class NoteUpdatedAtIsNotSetException : NotesException
{
    public override string Message { get => "Note UpdatedAt must be set first"; }
}
#endregion class Exceptions.NoteUpdatedAtIsNotSetException

#region class Exceptions.NoteCreatedAtIsNotSetException
public class NoteCreatedAtIsNotSetException : NotesException
{
    public override string Message { get => "Note CreatedAt must be set first"; }
}
#endregion class Exceptions.NoteCreatedAtIsNotSetException
