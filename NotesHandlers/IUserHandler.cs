#region "NotesHandlers/IUserHandler.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

namespace NullPointersEtc.NotesJournalApp.NotesHandlers
{
    using Guid = System.Guid;
    using User = NullPointersEtc.NotesJournalApp.UserEntity.User;
    using EmailAddress = System.ComponentModel.DataAnnotations.EmailAddressAttribute;

    #region "Handler interface INoteHandler"
    public interface IUserHandler
    {
        System.Threading.Tasks.Task<User>
            CreateAsync(string identifier,
                string display,
                [EmailAddress]
                string eMail);

        System.Threading.Tasks.Task<User>
            GetByGuidAsync(Guid userID);

        System.Threading.Tasks.Task<User>
            GetByIdentifierAsync(string identifier);

        System.Threading.Tasks.Task<User>
            GetByDisplayAsync(string display);

        System.Threading.Tasks.Task<
            System.Collections.Generic.IEnumerable<User>>
            GetAllAsync();

        System.Threading.Tasks.Task<User>
            UpdateAsync(Guid userID,
                string display,
                [EmailAddress]
                string eMail);

        System.Threading.Tasks.Task
            DeleteAsync(Guid userID);
    }
    #endregion "Handler interface INoteHandler"
}
#endregion "NotesHandler/IUserHandler.cs"
