#region class NoteRepository
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

#region dotnet add NotesStorage package Microsoft.EntityFrameworkCore
#region dotnet add NotesStorage package Microsoft.EntityFrameworkCore.Sqlite
using Microsoft.EntityFrameworkCore;
#endregion
#endregion

using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;

using INoteRepository =
    NullPointersEtc.NotesJournalApp.NoteEntity.INoteRepository;

namespace NullPointersEtc.NotesJournalApp.NotesStorage;

public class NoteRepository : INoteRepository
{
    public NoteRepository(NotesDbContext db)
    {
        myDb = db;
    }

    async System.Threading.Tasks.Task<Note>
        INoteRepository.CreateAsync(Note note)
    {
        myDb.Notes.Add(note);
        await myDb.SaveChangesAsync();
        return note;
    }

    System.Threading.Tasks.Task<Note>
        INoteRepository.GetAsync(Guid id)
        => myDb.Notes.FirstAsync(note => note.Id == id);

    async System.Threading.Tasks.Task<
        System.Collections.Generic.IEnumerable<Note>>
        INoteRepository.SearchAsync(string query)
        => await myDb.Notes.Where(
            note=>note.Title.Contains(query) ||
                note.Body.Contains(query)).ToListAsync();

    async System.Threading.Tasks.Task<Note>
        INoteRepository.UpdateAsync(Note note)
    {
        myDb.Notes.Update(note);
        await myDb.SaveChangesAsync();
        return note;
    }

    async Task INoteRepository.DeleteAsync(Guid id)
    {
        Note note = await myDb.Notes.FirstAsync(note => note.Id == id);
        myDb.Notes.Remove(note);
        await myDb.SaveChangesAsync();
    }

    private readonly NotesDbContext myDb;
}
#endregion class NoteRepository
