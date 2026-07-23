#region interface INoteHandler
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using Guid = System.Guid;

using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;

namespace NullPointersEtc.NotesJournalApp.NoteHandler;

public interface INoteHandler
{
    System.Threading.Tasks.Task<Note>
        CreateAsync(
            string title,
            string body);

    System.Threading.Tasks.Task<Note>
        GetAsync(Guid noteID);

    System.Threading.Tasks.Task<
        System.Collections.Generic.IEnumerable<Note>>
        SearchAsync(string query);

    System.Threading.Tasks.Task<Note>
        UpdateAsync(
            Guid noteID,
            string title,
            string body);

    System.Threading.Tasks.Task
        DeleteAsync(Guid noteID);
}
#endregion interface INoteHandler
