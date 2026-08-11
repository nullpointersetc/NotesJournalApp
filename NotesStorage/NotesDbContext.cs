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

#endregion
#endregion

namespace NullPointersEtc.NotesJournalApp.NotesStorage;

public abstract class NotesDbContext : DbContext
{
    public NotesDbContext(
        Microsoft.EntityFrameworkCore.DbContextOptions options)
        : base(options) { }

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
            entity.HasKey(note => note.NoteID);

            RelationalPropertyBuilderExtensions.UseCollation(
                entity.Property(note => note.Title)  .IsRequired(),
                collation: CaseInsensitiveCollation);

            entity.Property(note => note.Body)
                .IsRequired();
        });

        RelationalEntityTypeBuilderExtensions.ToTable(
            builder.Entity<Note>(), name: "NOTES");

        builder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.UserID);

            RelationalPropertyBuilderExtensions.UseCollation(
                entity.Property(user => user.UserName).IsRequired(),
                collation: CaseInsensitiveCollation);

            RelationalPropertyBuilderExtensions.UseCollation(
                entity.Property(user => user.DisplayName).IsRequired(),
                collation: CaseInsensitiveCollation);

            entity.Property(user => user.EMailAddress)
                .IsRequired();

            entity.HasIndex(user => user.UserName).IsUnique();
            entity.HasIndex(user => user.DisplayName).IsUnique();
        });

        RelationalEntityTypeBuilderExtensions.ToTable(
            builder.Entity<User>(), name: "USERS");
    }

    public abstract string CaseInsensitiveCollation { get; }
}

public class NotesDbContextForSqlite : NotesDbContext
{
    public NotesDbContextForSqlite(
        Microsoft.EntityFrameworkCore.DbContextOptions<
            NotesDbContextForSqlite> options)
        : base(options) { }

    public override string CaseInsensitiveCollation
    { get => "NOCASE"; }
}

public class NotesDbContextForSqlServer : NotesDbContext
{
    public NotesDbContextForSqlServer(
        Microsoft.EntityFrameworkCore.DbContextOptions<
            NotesDbContextForSqlServer> options)
        : base(options)
    {
    }

    public override string CaseInsensitiveCollation
    { get => "SQL_Latin1_General_CP1_CI_AS"; }
}

#endregion "NotesStorage/NotesDbContext.cs"
