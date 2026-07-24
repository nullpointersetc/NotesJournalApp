#region class UserHandler
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using IUserRepository =
    NullPointersEtc.NotesJournalApp.UserEntity.IUserRepository;

using User = NullPointersEtc.NotesJournalApp.UserEntity.User;

using DateTime = System.DateTime;
using Guid = System.Guid;

namespace NullPointersEtc.NotesJournalApp.UserHandler;

public class UserHandler : IUserHandler
{
    public UserHandler(IUserRepository repo)
    {
        myRepo = repo;
    }

    async System.Threading.Tasks.Task<User>
        IUserHandler.CreateAsync(
            string identifier,
            string display,
            string eMail)
    {
        DateTime now = DateTime.UtcNow;

        return await myRepo.CreateAsync(
            new User()
            {
                Id = Guid.NewGuid(),
                Display = display,
                Identifier = identifier,
                EMail = eMail,
                CreatedAt = now,
                UpdatedAt = now
            });
    }

    System.Threading.Tasks.Task<User>
        IUserHandler.GetByGuidAsync(Guid userID) =>
        myRepo.GetByIdAsync(userID);

    System.Threading.Tasks.Task<User>
        IUserHandler.GetByIdentifierAsync(
            string identifier) =>
        myRepo.GetByIdentifierAsync(identifier);

    System.Threading.Tasks.Task<User>
        IUserHandler.GetByDisplayAsync(
            string display) =>
        myRepo.GetByDisplayAsync(display);

    System.Threading.Tasks.Task<
        System.Collections.Generic.IEnumerable<User>>
        IUserHandler.GetAllAsync() =>
        myRepo.GetAllUsersAsync();

    async System.Threading.Tasks.Task<User>
        IUserHandler.UpdateAsync(
            Guid userID,
            string display,
            string eMail)
    {
        User user1 = await myRepo.GetByIdAsync(userID);

        user1.Display = display;
        user1.EMail = eMail;
        user1.UpdatedAt = DateTime.UtcNow;

        return await myRepo.UpdateAsync(user1);
    }

    public System.Threading.Tasks.Task
        DeleteAsync(Guid userID) =>
        myRepo.DeleteAsync(userID);

    private readonly IUserRepository myRepo;
}
#endregion class UserHandler
