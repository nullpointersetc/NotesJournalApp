#region class UserRepository
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

#region dotnet add NotesStorage package Microsoft.EntityFrameworkCore
#region dotnet add NotesStorage package Microsoft.EntityFrameworkCore.Sqlite
using Microsoft.EntityFrameworkCore;
#endregion
#endregion

using User = NullPointersEtc.NotesJournalApp.UserEntity.User;

using IUserRepository =
    NullPointersEtc.NotesJournalApp.UserEntity.IUserRepository;

namespace NullPointersEtc.NotesJournalApp.NotesStorage;

public class UserRepository : IUserRepository
{
    public UserRepository(NotesDbContext db1)
    {
        db = db1;
    }

    async System.Threading.Tasks.Task<User>
        IUserRepository.CreateAsync(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    System.Threading.Tasks.Task<User>
        IUserRepository.GetByIdAsync(Guid id)
        => db.Users.FirstAsync(user => user.Id == id);

    System.Threading.Tasks.Task<User>
        IUserRepository.GetByIdentifierAsync(string identifier)
        => db.Users.FirstAsync(user => user.Identifier == identifier);

    System.Threading.Tasks.Task<User>
        IUserRepository.GetByDisplayAsync(string display)
        => db.Users.FirstAsync(user => user.Display == display);

    async System.Threading.Tasks.Task<
        System.Collections.Generic.IEnumerable<User>>
        IUserRepository.GetAllUsersAsync()
        => await db.Users.ToListAsync();

    async System.Threading.Tasks.Task<User>
        IUserRepository.UpdateAsync(User user)
    {
        db.Users.Update(user);
        await db.SaveChangesAsync();
        return user;
    }

    async Task IUserRepository.DeleteAsync(Guid id)
    {
        User user = await db.Users.FirstAsync(user => user.Id == id);
        db.Users.Remove(user);
        await db.SaveChangesAsync();
    }

    private readonly NotesDbContext db;
}
#endregion class UserRepository
