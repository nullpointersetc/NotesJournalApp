#region "NotesDomain/NoteEntity.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240
#nullable enable

using Key = System.ComponentModel.DataAnnotations.KeyAttribute;
using StringLength = System.ComponentModel.DataAnnotations.StringLengthAttribute;

namespace NullPointersEtc.NotesJournalApp.NoteEntity
{

    #region "Entity class Note"
    public class Note
    {
        [property: Key]
        public System.Guid NoteID
        {
            get => noteId1 ?? throw new NoteIdIsNotSetException();
            set => noteId1 = value;
        }

        [StringLength(maximumLength: MAX_TITLE_LENGTH,
            MinimumLength = 1)]
        public string Title
        {
            get => title1 ?? throw new NoteTitleIsNotSetException();

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NoteTitleIsEmptyException();
                else if (value.Length > MAX_TITLE_LENGTH)
                    throw new NoteTitleIsTooLongException();
                else
                    title1 = value;
            }
        }

        [StringLength(maximumLength: MAX_BODY_LENGTH,
            MinimumLength = 1)]
        public string Body
        {
            get => body1 ?? throw new NoteBodyIsNotSetException();

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NoteBodyIsEmptyException();
                else if (value.Length > MAX_BODY_LENGTH)
                    throw new NoteBodyIsTooLongException();
                else
                    body1 = value;
            }
        }

        public DateTime CreatedAt
        {
            get => createdDate1 ?? throw new NoteCreationDateIsNotSetException();
            set => createdDate1 = value;
        }

        public DateTime LastUpdatedAt
        {
            get => lastUpdateDate1 ?? throw new NoteLastModifiedDateIsNotSetException();
            set => lastUpdateDate1 = value;
        }

        private System.Guid? noteId1 = null;
        private string? title1 = null;
        private string? body1 = null;
        private System.DateTime? createdDate1 = null;
        private System.DateTime? lastUpdateDate1 = null;

        public const int MAX_TITLE_LENGTH = 250;
        public const int MAX_BODY_LENGTH = 4000;
    }
    #endregion "Entity class Note"


    #region "Repository interface INoteRepository"
    public interface INoteRepository
    {
        System.Threading.Tasks.Task<Note> CreateAsync(Note note);

        System.Threading.Tasks.Task<Note> GetAsync(System.Guid id);

        System.Threading.Tasks.Task<
            System.Collections.Generic.IEnumerable<Note>>
            SearchAsync(string query);

        System.Threading.Tasks.Task<Note> UpdateAsync(Note note);

        System.Threading.Tasks.Task DeleteAsync(System.Guid id);
    }
    #endregion "Repository interface INoteRepository"


    #region "Exception class NoteIdIsNotSetException"
    public class NoteIdIsNotSetException : System.InvalidOperationException
    {
        public override string Message { get => "Note Id must be set first"; }
    }
    #endregion


    #region "Exception class NoteTitleIsNotSetException"
    public class NoteTitleIsNotSetException : System.InvalidOperationException
    {
        public override string Message { get => "Note Title must be set first"; }
    }
    #endregion


    #region "Exception class NoteTitleIsEmptyException"
    public class NoteTitleIsEmptyException : System.ArgumentException
    {
        public override string Message { get => "Note Title must not be empty"; }
    }
    #endregion


    #region "Exception class NoteTitleIsTooLongException"
    public class NoteTitleIsTooLongException : System.ArgumentException
    {
        public override string Message
        {
            get => "Note Title must be " +
                Note.MAX_TITLE_LENGTH + " characters or shorter";
        }
    }
    #endregion


    #region "Exception class NoteBodyIsNotSetException"
    public class NoteBodyIsNotSetException : System.InvalidOperationException
    {
        public override string Message { get => "Note Body must be set first"; }
    }
    #endregion


    #region "Exception class NoteBodyIsEmptyException"
    public class NoteBodyIsEmptyException : System.ArgumentException
    {
        public override string Message { get => "Note Body must not be empty"; }
    }
    #endregion


    #region "Exception class NoteBodyIsTooLongException"
    public class NoteBodyIsTooLongException : System.ArgumentException
    {
        public override string Message
        {
            get => "Note Body must be " +
                Note.MAX_BODY_LENGTH + " characters or shorter";
        }
    }
    #endregion


    #region "Exception class NoteLastModifiedDateIsNotSetException"
    public class NoteLastModifiedDateIsNotSetException : System.InvalidOperationException
    {
        public override string Message { get => "Note LastUpdatedAt must be set first"; }
    }
    #endregion


    #region "Exception class NoteCreationDateIsNotSetException"
    public class NoteCreationDateIsNotSetException : System.InvalidOperationException
    {
        public override string Message { get => "Note CreatedAt must be set first"; }
    }
    #endregion
}
#endregion "NotesDomain/NoteEntity.cs"
