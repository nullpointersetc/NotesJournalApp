#region "NoteRepository.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using Guid = System.Guid;
using Queryable = System.Linq.Queryable;
using Task = System.Threading.Tasks.Task;

using TaskReturningNote = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.NoteEntity.Note>;

using TaskReturningNotes = System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.NoteEntity.Note>>;

using INoteRepository =
    NullPointersEtc.NotesJournalApp.NoteEntity.INoteRepository;

#region "dotnet add package Microsoft.EntityFrameworkCore --version 8.0.23"

using EntityFrameworkQueryableExtensions =
    Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

#endregion

namespace NullPointersEtc.NotesJournalApp.NotesStorage;

public sealed class NoteRepository : INoteRepository
{
    public NoteRepository(NotesDbContextForSqlite db)
    {
        myDB = db;
    }


    public NoteRepository(NotesDbContextForSqlServer db)
    {
        myDB = db;
    }


    public async TaskReturningNote CreateNoteAsync(Note note)
    {
        myDB.Notes.Add(note);
        await myDB.SaveChangesAsync();
        return note;
    }


    public async TaskReturningNotes GetAllNotesAsync()
        => await EntityFrameworkQueryableExtensions.ToListAsync(myDB.Notes);


    public TaskReturningNote GetNoteByIdAsync(Guid noteID)
        => EntityFrameworkQueryableExtensions.FirstAsync(
            myDB.Notes, predicate: note => note.NoteID == noteID);


    public async TaskReturningNotes SearchNotesAsync(
            string query)
        => await EntityFrameworkQueryableExtensions.ToListAsync(
            Queryable.Where(myDB.Notes,
                predicate: note => note.Title.Contains(query)
                    || note.Body.Contains(query)));


    public async TaskReturningNote UpdateNoteAsync(Note note)
    {
        myDB.Notes.Update(note);
        await myDB.SaveChangesAsync();
        return note;
    }


    public async Task DeleteNoteAsync(Guid noteID)
    {
        Note note1 = await EntityFrameworkQueryableExtensions.FirstAsync(
            myDB.Notes, predicate: note => note.NoteID == noteID);

        myDB.Notes.Remove(note1);
        await myDB.SaveChangesAsync();
    }

    private readonly NotesDbContext myDB;
}

#endregion "NoteRepository.cs"
