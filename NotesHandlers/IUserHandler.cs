#region interface IUserHandler
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using Guid = System.Guid;
using User = NullPointersEtc.NotesJournalApp.UserEntity.User;
using EmailAddress = System.ComponentModel.DataAnnotations.EmailAddressAttribute;

namespace NullPointersEtc.NotesJournalApp.UserHandler;

public interface IUserHandler
{
    System.Threading.Tasks.Task<User>
        CreateAsync(
            string identifier,
            string display,
            [EmailAddress]
            string eMail);

    System.Threading.Tasks.Task<User>
        GetByGuidAsync(Guid userID);

    System.Threading.Tasks.Task<User>
        GetByIdentifierAsync(string identifier);

    System.Threading.Tasks.Task<
        System.Collections.Generic.IEnumerable<User>>
        GetAllAsync();

    System.Threading.Tasks.Task<User>
        UpdateAsync(
            Guid userID,
            string display,
            [EmailAddress]
            string eMail);

    System.Threading.Tasks.Task
        DeleteAsync(Guid userID);
}
#endregion interface IUserHandler
