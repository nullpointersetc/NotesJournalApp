#region "NotesHandlers/NoteHandler.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using DateTime = System.DateTime;
using Guid = System.Guid;
using Task = System.Threading.Tasks.Task;

using INoteRepository =
    NullPointersEtc.NotesJournalApp.NoteEntity.INoteRepository;

using TaskReturningNote = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.NoteEntity.Note>;

using TaskReturningNotes = System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.NoteEntity.Note>>;

namespace NullPointersEtc.NotesJournalApp.NotesHandlers;

public sealed class NoteHandler : INoteHandler
{
    public NoteHandler(INoteRepository repo)
    {
        myRepo = repo;
    }


    public async TaskReturningNote
        CreateNoteWithHandlerAsync(
            string title, string body)
    {
        DateTime now = DateTime.UtcNow;

        return await myRepo.CreateNoteAsync(new Note()
            {
                NoteID = Guid.NewGuid(),
                Title = title,
                Body = body,
                CreatedAt = now,
                LastModifiedAt = now
            });
    }


    public TaskReturningNotes GetAllNotesWithHandlerAsync()
        => myRepo.GetAllNotesAsync();


    public TaskReturningNote GetNoteFromNoteIdWithHandlerAsync(
        Guid noteID)
        => myRepo.GetNoteByIdAsync(noteID);


    public TaskReturningNotes SearchNotesWithHandlerAsync(
        string query)
        => myRepo.SearchNotesAsync(query);


    public async TaskReturningNote UpdateNoteWithHandlerAsync(
        Guid noteID,
        string title, string body)
    {
        Note note1 = await myRepo.GetNoteByIdAsync(noteID);
        note1.Title = title;
        note1.Body = body;
        note1.LastModifiedAt = DateTime.UtcNow;
        return await myRepo.UpdateNoteAsync(note1);
    }


    public Task DeleteNoteWithHandlerAsync(Guid noteID)
        => myRepo.DeleteNoteAsync(noteID);


    private readonly INoteRepository myRepo;
}

#endregion "NotesHandlers/NoteHandler.cs"
