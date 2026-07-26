#region "NoteRepository.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

/* To include this "using", you must execute:
**
** dotnet add NotesStorage package Microsoft.EntityFrameworkCore
** dotnet add NotesStorage package Microsoft.EntityFrameworkCore.Sqlite
** dotnet add NotesStorage package Microsoft.EntityFrameworkCore.SqlServer
*/

using Microsoft.EntityFrameworkCore;

using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;

using INoteRepository =
    NullPointersEtc.NotesJournalApp.NoteEntity.INoteRepository;

namespace NullPointersEtc.NotesJournalApp.NotesStorage
{
    public class NoteRepository : INoteRepository
    {
        public NoteRepository(NotesDbContext db)
        {
            db1 = db;
        }

        async System.Threading.Tasks.Task<Note>
            INoteRepository.CreateAsync(Note note)
        {
            db1.Notes.Add(note);
            await db1.SaveChangesAsync();
            return note;
        }

        async System.Threading.Tasks.Task<
            System.Collections.Generic.IEnumerable<Note>>
            INoteRepository.GetAllAsync()
            => await db1.Notes.ToListAsync();

        System.Threading.Tasks.Task<Note>
            INoteRepository.GetAsync(Guid noteID)
            => db1.Notes.FirstAsync(note => note.NoteID == noteID);

        async System.Threading.Tasks.Task<
            System.Collections.Generic.IEnumerable<Note>> INoteRepository.SearchAsync(
                string query)
            => await db1.Notes.Where(
                note => note.Title.Contains(query)
                    || note.Body.Contains(query)).ToListAsync();

        async System.Threading.Tasks.Task<Note>
            INoteRepository.UpdateAsync(Note note)
        {
            db1.Notes.Update(note);
            await db1.SaveChangesAsync();
            return note;
        }

        async System.Threading.Tasks.Task
            INoteRepository.DeleteAsync(Guid noteID)
        {
            Note note1 = await db1.Notes.FirstAsync(
                note => note.NoteID == noteID);

            db1.Notes.Remove(note1);
            await db1.SaveChangesAsync();
        }
        
        private readonly NotesDbContext db1;
    }
}

#endregion "NoteRepository.cs"
