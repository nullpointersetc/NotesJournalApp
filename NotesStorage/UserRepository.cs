#region "UserRepository.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using User = NullPointersEtc.NotesJournalApp.UserEntity.User;
using Guid = System.Guid;
using Task = System.Threading.Tasks.Task;

using TaskReturningUser = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.UserEntity.User>;

using TaskReturningUsers = System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.UserEntity.User>>;

using IUserRepository =
    NullPointersEtc.NotesJournalApp.UserEntity.IUserRepository;

using EntityFrameworkQueryableExtensions =
    Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

namespace NullPointersEtc.NotesJournalApp.NotesStorage;

public sealed class UserRepository : IUserRepository
{
    public UserRepository(NotesDbContextForSqlite db)
    {
        myDB = db;
    }


    public UserRepository(NotesDbContextForSqlServer db)
    {
        myDB = db;
    }


    public async TaskReturningUser
        CreateUserAsync(User user)
    {
        myDB.Users.Add(user);
        await myDB.SaveChangesAsync();
        return user;
    }


    public TaskReturningUser GetUserByUserIdAsync(Guid userID)
        => EntityFrameworkQueryableExtensions.FirstAsync(
            myDB.Users, predicate: user => user.UserID == userID);


    public TaskReturningUser GetUserByUserNameAsync(
            string userName)
        => EntityFrameworkQueryableExtensions.FirstAsync(
            myDB.Users,
            predicate: user => user.UserName == userName);


    public TaskReturningUser GetUserByDisplayNameAsync(
        string displayName)
        => EntityFrameworkQueryableExtensions.FirstAsync(
            myDB.Users,
            predicate: user => user.DisplayName == displayName);


    public async TaskReturningUsers GetAllUsersAsync()
        => await EntityFrameworkQueryableExtensions.ToListAsync<User>(
            myDB.Users);


    public async TaskReturningUser UpdateUserAsync(User user)
    {
        myDB.Users.Update(user);
        await myDB.SaveChangesAsync();
        return user;
    }


    public async Task DeleteUserAsync(
        Guid userID)
    {
        User user = await EntityFrameworkQueryableExtensions.FirstAsync(
            myDB.Users,
            predicate: user => user.UserID == userID);

        myDB.Users.Remove(user);
        await myDB.SaveChangesAsync();
    }


    private readonly NotesDbContext myDB;
}

#endregion "UserRepository.cs"
