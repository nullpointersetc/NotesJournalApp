#region interface INoteRepository
#pragma warning disable IDE0240
#nullable enable

using Guid = System.Guid;

using Task_Note = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.Entities.Note>;

using Task_Optional_Note = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.Entities.Note?>;

using Task_IEnumerable_Note =
    System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
    NullPointersEtc.NotesJournalApp.Entities.Note>>;

using Task_Void = System.Threading.Tasks.Task;

namespace NullPointersEtc.NotesJournalApp.Entities;

public interface INoteRepository
{
    Task_Note CreateAsync(Note note);
    Task_Optional_Note GetAsync(Guid id);
    Task_IEnumerable_Note SearchAsync(string query);
    Task_Note UpdateAsync(Note note);
    Task_Void DeleteAsync(Guid id);
}
#endregion interface INoteRepository
