#region "UserRepository.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

/* To include this "using", you must execute:
**
** dotnet add NotesStorage package Microsoft.EntityFrameworkCore
** dotnet add NotesStorage package Microsoft.EntityFrameworkCore.Sqlite
** dotnet add NotesStorage package Microsoft.EntityFrameworkCore.SqlServer
*/

using Microsoft.EntityFrameworkCore;

using User = NullPointersEtc.NotesJournalApp.UserEntity.User;

using IUserRepository =
    NullPointersEtc.NotesJournalApp.UserEntity.IUserRepository;

namespace NullPointersEtc.NotesJournalApp.NotesStorage
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(NotesDbContextForSqlite db)
        {
            db1 = db;
        }

        public UserRepository(NotesDbContextForSqlServer db)
        {
            db1 = db;
        }

        async System.Threading.Tasks.Task<User>
            IUserRepository.CreateAsync(User user)
        {
            db1.Users.Add(user);
            await db1.SaveChangesAsync();
            return user;
        }

        System.Threading.Tasks.Task<User>
            IUserRepository.GetByIdAsync(Guid userID)
            => db1.Users.FirstAsync(user => user.UserID == userID);

        System.Threading.Tasks.Task<User>
            IUserRepository.GetByIdentifierAsync(
                string identifier)
            => db1.Users.FirstAsync(user => user.UserName==identifier);

        System.Threading.Tasks.Task<User>
            IUserRepository.GetByDisplayAsync(string display)
            => db1.Users.FirstAsync(user => user.Display == display);

        async System.Threading.Tasks.Task<
            System.Collections.Generic.IEnumerable<User>>
            IUserRepository.GetAllUsersAsync()
            => await db1.Users.ToListAsync();

        async System.Threading.Tasks.Task<User>
            IUserRepository.UpdateAsync(User user)
        {
            db1.Users.Update(user);
            await db1.SaveChangesAsync();
            return user;
        }

        async Task IUserRepository.DeleteAsync(
            Guid id)
        {
            User user = await db1.Users.FirstAsync(user => user.UserID == id);
            db1.Users.Remove(user);
            await db1.SaveChangesAsync();
        }

        private readonly NotesDbContext db1;

    }
}

#endregion "UserRepository.cs"
