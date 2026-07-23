#region class NoteHandler
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using INoteRepository =
    NullPointersEtc.NotesJournalApp.NoteEntity.INoteRepository;

using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;

using DateTime = System.DateTime;
using Guid = System.Guid;

namespace NullPointersEtc.NotesJournalApp.NoteHandler;

public class NoteHandler : INoteHandler
{
    public NoteHandler(INoteRepository repo)
    {
        myRepo = repo;
    }

    async System.Threading.Tasks.Task<Note>
        INoteHandler.CreateAsync(
            string title,
            string body)
    {
        DateTime now = DateTime.UtcNow;

        return await myRepo.CreateAsync(
            new Note()
            {
                Id = Guid.NewGuid(),
                Title = title,
                Body = body,
                CreatedAt = now,
                UpdatedAt = now
            });
    }

    System.Threading.Tasks.Task<Note>
        INoteHandler.GetAsync(Guid noteID) =>
        myRepo.GetAsync(noteID);

    System.Threading.Tasks.Task<
        System.Collections.Generic.IEnumerable<Note>>
        INoteHandler.SearchAsync(string query) =>
        myRepo.SearchAsync(query);

    async System.Threading.Tasks.Task<Note>
        INoteHandler.UpdateAsync(
            Guid noteID,
            string title,
            string body)
    {
        Note existing = await myRepo.GetAsync(noteID);
        existing.Title = title;
        existing.Body = body;
        existing.UpdatedAt = DateTime.UtcNow;

        return await myRepo.UpdateAsync(existing);
    }

    System.Threading.Tasks.Task
        INoteHandler.DeleteAsync(Guid noteID) =>
        myRepo.DeleteAsync(noteID);

    private readonly INoteRepository myRepo;
}
#endregion class NoteHandler
