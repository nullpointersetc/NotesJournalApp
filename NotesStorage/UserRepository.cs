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

#region "dotnet add package Microsoft.EntityFrameworkCore --version 8.0.23"
using Microsoft.EntityFrameworkCore;
#endregion

namespace NullPointersEtc.NotesJournalApp.NotesStorage;

public sealed class UserRepository : IUserRepository
{
    public UserRepository(NotesDbContext db)
    {
        myDB = db;
    }


    public async TaskReturningUser CreateUserAsync(User user)
    {
        myDB.Users.Add(user);
        await myDB.SaveChangesAsync();
        return user;
    }


    public TaskReturningUser GetUserByUserIdAsync(Guid userID)
        => myDB.Users.FirstAsync<User>(
            predicate: user => user.UserID == userID);


    public TaskReturningUser GetUserByUserNameAsync(
            string userName)
        => myDB.Users.FirstAsync<User>(
                predicate: user => user.UserName == userName);


    public TaskReturningUser GetUserByDisplayNameAsync(
        string displayName)
        => myDB.Users.FirstAsync<User>(
            predicate: user => user.DisplayName == displayName);


    public async TaskReturningUsers GetAllUsersAsync()
        => await myDB.Users.ToListAsync<User>();


    public async TaskReturningUser UpdateUserAsync(User user)
    {
        myDB.Users.Update(user);
        await myDB.SaveChangesAsync();
        return user;
    }


    public async Task DeleteUserAsync(
        Guid userID)
    {
        User user = await myDB.Users.FirstAsync<User>(
            predicate: user => user.UserID == userID);

        myDB.Users.Remove(user);
        await myDB.SaveChangesAsync();
    }


    private readonly NotesDbContext myDB;
}

#endregion "UserRepository.cs"
