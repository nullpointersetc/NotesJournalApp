#region "NotesHandlers/INoteHandler.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240
#nullable enable

using Guid = System.Guid;
using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using Task = System.Threading.Tasks.Task;

using TaskReturningNote = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.NoteEntity.Note>;

using TaskReturningNotes = System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.NoteEntity.Note>>;

namespace NullPointersEtc.NotesJournalApp.NotesHandlers
{
    public interface INoteHandler
    {
        TaskReturningNote CreateNoteWithHandlerAsync(
            string title, string body);

        TaskReturningNotes GetAllNotesWithHandlerAsync();

        TaskReturningNote GetNoteFromNoteIdWithHandlerAsync(
            Guid noteID);

        TaskReturningNotes SearchNotesWithHandlerAsync(
            string query);

        TaskReturningNote UpdateNoteWithHandlerAsync(
            Guid noteID,
            string title, string body);

        Task DeleteNoteWithHandlerAsync(Guid noteID);
    }
}
#endregion "NotesHandlers/INoteHandler.cs"
