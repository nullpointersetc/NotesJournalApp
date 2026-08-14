#region "NoteRepository.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using Guid = System.Guid;
using System.Linq;
using Task = System.Threading.Tasks.Task;

using TaskReturningNote = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.NoteEntity.Note>;

using TaskReturningNotes = System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.NoteEntity.Note>>;

using INoteRepository =
    NullPointersEtc.NotesJournalApp.NoteEntity.INoteRepository;

#region "dotnet add package Microsoft.EntityFrameworkCore --version 8.0.23"
using Microsoft.EntityFrameworkCore;
#endregion

namespace NullPointersEtc.NotesJournalApp.NotesStorage;

public sealed class NoteRepository : INoteRepository
{
    public NoteRepository(NotesDbContext db)
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
        => await myDB.Notes.ToListAsync<Note>();


    public TaskReturningNote GetNoteByIdAsync(Guid noteID)
        => myDB.Notes.FirstAsync<Note>(predicate: note => note.NoteID == noteID);


    public async TaskReturningNotes SearchNotesAsync(
            string query)
        => await myDB.Notes.Where<Note>(
                predicate: note => note.Title.Contains(query)
                    || note.Body.Contains(query)).ToListAsync<Note>();


    public async TaskReturningNote UpdateNoteAsync(Note note)
    {
        myDB.Notes.Update(note);
        await myDB.SaveChangesAsync();
        return note;
    }


    public async Task DeleteNoteAsync(Guid noteID)
    {
        Note note1 = await myDB.Notes.FirstAsync(
            predicate: note => note.NoteID == noteID);

        myDB.Notes.Remove(note1);
        await myDB.SaveChangesAsync();
    }

    private readonly NotesDbContext myDB;
}

#endregion "NoteRepository.cs"
