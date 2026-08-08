#region "NotesHandlers/UserHandler.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using IUserRepository =
    NullPointersEtc.NotesJournalApp.UserEntity.IUserRepository;

using User = NullPointersEtc.NotesJournalApp.UserEntity.User;
using DateTime = System.DateTime;
using Guid = System.Guid;
using Task = System.Threading.Tasks.Task;

using TaskReturningUser = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.UserEntity.User>;

using TaskReturningUsers = System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.UserEntity.User>>;

namespace NullPointersEtc.NotesJournalApp.NotesHandlers;

public sealed class UserHandler : IUserHandler
{
    public UserHandler(IUserRepository repo)
    {
        myRepo = repo;
    }

    public async TaskReturningUser CreateUserWithHandlerAsync(
        string userName,
        string displayName,
        string eMailAddress)
    {
        DateTime now = DateTime.UtcNow;

        return await myRepo.CreateUserAsync(
            new User()
            {
                UserID = Guid.NewGuid(),
                UserName = userName,
                DisplayName = displayName,
                EMailAddress = eMailAddress,
                CreatedAt = now,
                LastModifiedAt = now
            });
    }


    public TaskReturningUser GetUserFromUserIdWithHandlerAsync(
        Guid userID)
        => myRepo.GetUserByUserIdAsync(userID);


    public TaskReturningUser GetUserFromUserNameWithHandlerAsync(
        string userName)
        => myRepo.GetUserByUserNameAsync(userName);


    public TaskReturningUser GetUserFromDisplayNameWithHandlerAsync(
        string displayName)
        => myRepo.GetUserByDisplayNameAsync(displayName);


    public TaskReturningUsers GetAllUsersWithHandlerAsync()
        => myRepo.GetAllUsersAsync();


    public async TaskReturningUser UpdateUserWithHandlerAsync(
        Guid userID,
        string displayName,
        string eMailAddress)
    {
        User user1 = await myRepo.GetUserByUserIdAsync(userID);
        user1.DisplayName = displayName;
        user1.EMailAddress = eMailAddress;
        user1.LastModifiedAt = DateTime.UtcNow;
        return await myRepo.UpdateUserAsync(user1);
    }


    public Task DeleteWithHandlerAsync(Guid userID) =>
            myRepo.DeleteUserAsync(userID);


    private readonly IUserRepository myRepo;
}
#endregion "NotesHandlers/UserHandler.cs"
