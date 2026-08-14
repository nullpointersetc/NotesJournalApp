#region "NotesStorage/NotesDbContext.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using User = NullPointersEtc.NotesJournalApp.UserEntity.User;

#region "dotnet add package Microsoft.EntityFrameworkCore --version 8.0.23"

using DbContext = Microsoft.EntityFrameworkCore.DbContext;
using ModelBuilder = Microsoft.EntityFrameworkCore.ModelBuilder;

#endregion

#region "dotnet package add Microsoft.EntityFrameworkCore.Sqlite --version 8.0.23"
#region "dotnet package add Microsoft.EntityFrameworkCore.SqlServer --version 8.0.23"

using RelationalPropertyBuilderExtensions =
    Microsoft.EntityFrameworkCore.RelationalPropertyBuilderExtensions;

using RelationalEntityTypeBuilderExtensions =
    Microsoft.EntityFrameworkCore.RelationalEntityTypeBuilderExtensions;
using Microsoft.EntityFrameworkCore;

#endregion
#endregion

namespace NullPointersEtc.NotesJournalApp.NotesStorage;

public sealed class NotesDbContext : DbContext
{
    public NotesDbContext(
        Microsoft.EntityFrameworkCore.DbContextOptions options,
        MoreOptionsForNotesDbContext moreOptions)
        : base(options)
    {
        privateOptions = moreOptions;
    }


    public Microsoft.EntityFrameworkCore.DbSet<Note>
        Notes
    { get => Set<Note>(); }

    public Microsoft.EntityFrameworkCore.DbSet<User>
        Users
    { get => Set<User>(); }

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Note>(entity =>
        {
            entity.ToTable(name: "NOTES");
            entity.HasKey(note => note.NoteID);

            entity.Property(note => note.Title).IsRequired()
                .UseCollation(privateOptions.CaseInsensitiveCollation);

            entity.Property(note => note.Body).IsRequired();
        });

        builder.Entity<User>(entity =>
        {
            entity.ToTable(name: "USERS");
            entity.HasKey(user => user.UserID);

            entity.Property(user => user.UserName).IsRequired()
                .UseCollation(privateOptions.CaseInsensitiveCollation);

            entity.Property(user => user.DisplayName).IsRequired()
                .UseCollation(privateOptions.CaseInsensitiveCollation);

            entity.Property(user => user.EMailAddress).IsRequired();

            entity.HasIndex(user => user.UserName).IsUnique();
            entity.HasIndex(user => user.DisplayName).IsUnique();
        });
    }

    private readonly MoreOptionsForNotesDbContext privateOptions;
}

public sealed class MoreOptionsForNotesDbContext
{
    public static MoreOptionsForNotesDbContext ForSqlServer()
        => new(caseInsensitiveCollation: "SQL_Latin1_General_CP1_CI_AS");

    public static MoreOptionsForNotesDbContext ForSqlite()
        => new(caseInsensitiveCollation: "NOCASE");

    public MoreOptionsForNotesDbContext(string caseInsensitiveCollation)
    {
        collation = caseInsensitiveCollation;
    }

    public string CaseInsensitiveCollation
    {
        get => collation;
    }

    private readonly string collation;
}
#endregion "NotesStorage/NotesDbContext.cs"
