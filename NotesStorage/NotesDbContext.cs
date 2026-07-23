#region class HandlerSomething
#pragma warning disable IDE0130, IDE0240, IDE0290
#nullable enable

#region dotnet add NotesStorage package Microsoft.EntityFrameworkCore
#region dotnet add NotesStorage package Microsoft.EntityFrameworkCore.Sqlite
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
using ModelBuilder = Microsoft.EntityFrameworkCore.ModelBuilder;
#endregion
#endregion


using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using User = NullPointersEtc.NotesJournalApp.UserEntity.User;

namespace NullPointersEtc.NotesJournalApp.NotesStorage;

public class NotesDbContext : DbContext
{
    public NotesDbContext(
        Microsoft.EntityFrameworkCore.DbContextOptions<NotesDbContext>
            options)
    : base(options) { }

    public Microsoft.EntityFrameworkCore.DbSet<Note> Notes => Set<Note>();

    public Microsoft.EntityFrameworkCore.DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        bool IsSqlServer =
            Database.ProviderName?.Contains("SqlServer") ?? false;

        bool IsSqlite =
            Database.ProviderName?.Contains("Sqlite") ?? false;

        string SqlServerCollation = "SQL_Latin1_General_CP1_CI_AS";
        string SqliteCollation = "NOCASE";

        model.Entity<Note>().ToTable("NOTES");
        model.Entity<User>().ToTable("USERS");

        model.Entity<Note>(entity =>
        {
            entity.HasKey(note => note.Id);
            entity.Property(note => note.Title).IsRequired();
            entity.Property(note => note.Body).IsRequired();
        });

        model.Entity<User>(entity =>
        {
            entity.HasKey(user => user.Id);

            entity.Property(user => user.Identifier).IsRequired();

            if (IsSqlite)
                entity.Property(user => user.Identifier).UseCollation(SqliteCollation);
            else if (IsSqlServer)
                entity.Property(user => user.Identifier).UseCollation(SqlServerCollation);

            entity.Property(user => user.Display).IsRequired();

            if (IsSqlite)
                entity.Property(user => user.Display).UseCollation(SqliteCollation);
            else if (IsSqlServer)
                entity.Property(user => user.Display).UseCollation(SqlServerCollation);

            entity.Property(user => user.EMail).IsRequired();

            entity.HasIndex(user => user.Identifier).IsUnique();
            entity.HasIndex(user => user.Display).IsUnique();
        });
    }
}
#endregion class HandlerSomething
