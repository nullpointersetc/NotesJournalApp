#region class NotesDbContext
#pragma warning disable IDE0130, IDE0240, IDE0290
#nullable enable

#region dotnet add NotesStorage package Microsoft.EntityFrameworkCore
#region dotnet add NotesStorage package Microsoft.EntityFrameworkCore.Sqlite
#region dotnet add NotesStorage package Microsoft.EntityFrameworkCore.SqlServer
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
using ModelBuilder = Microsoft.EntityFrameworkCore.ModelBuilder;
#endregion
#endregion
#endregion

using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using User = NullPointersEtc.NotesJournalApp.UserEntity.User;

namespace NullPointersEtc.NotesJournalApp.NotesStorage;

public class NotesDbContext : DbContext
{
    public NotesDbContext(
        Microsoft.EntityFrameworkCore.DbContextOptions<NotesDbContext> options,
        NotesDbContextParams contextParams)
    : base(options)
    {
        myContextParams = contextParams;
    }

    public Microsoft.EntityFrameworkCore.DbSet<Note> Notes => Set<Note>();

    public Microsoft.EntityFrameworkCore.DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder model)
    {
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

            if (myContextParams.UseSQLite)
                entity.Property(user => user.Identifier).UseCollation(SqliteCollation);

            if (myContextParams.UseSqlServer)
                entity.Property(user => user.Identifier).UseCollation(SqlServerCollation);

            entity.Property(user => user.Display).IsRequired();

            if (myContextParams.UseSQLite)
                entity.Property(user => user.Display).UseCollation(SqliteCollation);

            if (myContextParams.UseSqlServer)
                entity.Property(user => user.Display).UseCollation(SqlServerCollation);

            entity.Property(user => user.EMail).IsRequired();

            entity.HasIndex(user => user.Identifier).IsUnique();
            entity.HasIndex(user => user.Display).IsUnique();
        });
    }

    private readonly NotesDbContextParams myContextParams;
}


public class NotesDbContextParams
{
    public NotesDbContextParams(
        bool useSqlServer)
    {
        myUseSqlServer = useSqlServer;
    }

    public bool UseSqlServer { get => myUseSqlServer; }
    public bool UseSQLite { get => !myUseSqlServer; }
    private readonly bool myUseSqlServer;
}
#endregion class NotesDbContext
