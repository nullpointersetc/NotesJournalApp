#region "NotesHandlers/NoteHandler.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

namespace NullPointersEtc.NotesJournalApp.NotesHandlers
{
    using INoteRepository =
        NullPointersEtc.NotesJournalApp.NoteEntity.INoteRepository;

    using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
    using DateTime = System.DateTime;
    using Guid = System.Guid;

    #region "Handler interface INoteHandler"
    public class NoteHandler : INoteHandler
    {
        public NoteHandler(INoteRepository repo)
        {
            repo1 = repo;
        }


        async System.Threading.Tasks.Task<Note>
            INoteHandler.CreateAsync(string title, string body)
        {
            DateTime now = DateTime.UtcNow;

            return await repo1.CreateAsync(
                new Note()
                {
                    NoteID = Guid.NewGuid(),
                    Title = title,
                    Body = body,
                    CreatedAt = now,
                    LastUpdatedAt = now
                });
        }


        System.Threading.Tasks.Task<
            System.Collections.Generic.IEnumerable<Note>>
            INoteHandler.GetAllNotesAsync()
            => repo1.GetAllAsync();


        System.Threading.Tasks.Task<Note>
            INoteHandler.GetNotesAsync(Guid noteID)
            => repo1.GetAsync(noteID);


        System.Threading.Tasks.Task<
            System.Collections.Generic.IEnumerable<Note>>
            INoteHandler.SearchAsync(string query)
            => repo1.SearchAsync(query);


        async System.Threading.Tasks.Task<Note>
            INoteHandler.UpdateAsync(Guid noteID,
                string title, string body)
        {
            Note note1 = await repo1.GetAsync(noteID);
            note1.Title = title;
            note1.Body = body;
            note1.LastUpdatedAt = DateTime.UtcNow;
            return await repo1.UpdateAsync(note1);
        }


        System.Threading.Tasks.Task
            INoteHandler.DeleteAsync(Guid noteID)
            => repo1.DeleteAsync(noteID);


        private readonly INoteRepository repo1;
    }
    #endregion "Handler interface INoteHandler"
}
#endregion "NotesHandlers/NoteHandler.cs"
