#region interface IUserRepository
#pragma warning disable IDE0130
#pragma warning disable IDE0240
#nullable enable

using Guid = System.Guid;

using Task_User = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.Entities.User>;

using Task_Optional_User = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.Entities.User?>;

using Task_IEnumerable_User =
    System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
    NullPointersEtc.NotesJournalApp.Entities.User>>;

using Task = System.Threading.Tasks.Task;

namespace NullPointersEtc.NotesJournalApp.Entities;

public interface IUserRepository
{
    Task_User CreateAsync(User user);
    Task_Optional_User GetByIdAsync(Guid userID);
    Task_Optional_User GetByIdentifierAsync(
        string identifier);
    Task_Optional_User GetByDisplayAsync(
        string display);
    Task_IEnumerable_User GetAllUsersAsync();
    Task_User UpdateAsync(User user);
    Task DeleteAsync(Guid userID);
}
#endregion interface IUserRepository
