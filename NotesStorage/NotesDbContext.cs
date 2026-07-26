#region "NotesDbContext.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

/* To include this "using", you must execute:
**
** dotnet add NotesStorage package Microsoft.EntityFrameworkCore
** dotnet add NotesStorage package Microsoft.EntityFrameworkCore.Sqlite
** dotnet add NotesStorage package Microsoft.EntityFrameworkCore.SqlServer
*/

using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using User = NullPointersEtc.NotesJournalApp.UserEntity.User;

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

            entity.Property(note => note.Title)
                .IsRequired()
                .UseCollation(CaseInsensitiveCollation);

            entity.Property(note => note.Body)
                .IsRequired();
        }).Entity<Note>().ToTable("NOTES");

        builder.Entity<User>(entity =>
        {
            entity.HasKey(user => user.UserID);

            entity.Property(user => user.UserName)
                .IsRequired()
                .UseCollation(CaseInsensitiveCollation);

            entity.Property(user => user.Display)
                .IsRequired()
                .UseCollation(CaseInsensitiveCollation);

            entity.Property(user => user.EMail)
                .IsRequired();
            
            entity.HasIndex(user => user.UserName).IsUnique();
            entity.HasIndex(user => user.Display).IsUnique();
        }).Entity<User>().ToTable("USERS");
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

#endregion "NotesDbContext.cs"
