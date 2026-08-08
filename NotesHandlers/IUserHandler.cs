#region "NotesHandlers/IUserHandler.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using Guid = System.Guid;
using EmailAddress = System.ComponentModel.DataAnnotations.EmailAddressAttribute;
using Task = System.Threading.Tasks.Task;

using TaskReturningUser = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.UserEntity.User>;

using TaskReturningUsers = System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.UserEntity.User>>;

namespace NullPointersEtc.NotesJournalApp.NotesHandlers;

public interface IUserHandler
{
    TaskReturningUser CreateUserWithHandlerAsync(
        string userName,
        string displayName,
        [EmailAddress]
        string eMail);

    TaskReturningUser GetUserFromUserIdWithHandlerAsync(
        Guid userID);

    TaskReturningUser GetUserFromUserNameWithHandlerAsync(
        string userName);

    TaskReturningUser GetUserFromDisplayNameWithHandlerAsync(
        string displayName);

    TaskReturningUsers GetAllUsersWithHandlerAsync();

    TaskReturningUser UpdateUserWithHandlerAsync(
        Guid userID,
        string displayName,
        [EmailAddress]
        string eMailAddress);

    Task DeleteWithHandlerAsync(
        Guid userID);
}
#endregion "NotesHandler/IUserHandler.cs"
