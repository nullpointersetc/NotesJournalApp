﻿#region "NotesHandlers_Int/NoteHandlerIntTests.cs"
#pragma warning disable IDE0130, xUnit2012

using Assert = Xunit.Assert;
using Fact = Xunit.FactAttribute;
using Guid = System.Guid;
using InvalidOperationException = System.InvalidOperationException;
using NotesDbContextForSqlite = NullPointersEtc.NotesJournalApp.NotesStorage.NotesDbContextForSqlite;
using NoteRepository = NullPointersEtc.NotesJournalApp.NotesStorage.NoteRepository;
using NoteHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.NoteHandler;
using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using Task = System.Threading.Tasks.Task;

using SqliteDbContextOptionsBuilderExtensions =
    Microsoft.EntityFrameworkCore.SqliteDbContextOptionsBuilderExtensions;

using DbContextOptionsBuilderNotesDbContextForSqlite =
    Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<
        NullPointersEtc.NotesJournalApp.NotesStorage.NotesDbContextForSqlite>;

using RelationalDatabaseFacadeExtensions =
    Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions;

using Notes = System.Collections.Generic.IEnumerable<
    NullPointersEtc.NotesJournalApp.NoteEntity.Note>;

namespace NullPointersEtc.NotesJournalApp.NotesHandlers_IntegrationTests;

public class NoteHandlerIntegrationTests
{
    private static NotesDbContextForSqlite CreateDb()
    {
        DbContextOptionsBuilderNotesDbContextForSqlite options =
            SqliteDbContextOptionsBuilderExtensions.UseSqlite(
            new DbContextOptionsBuilderNotesDbContextForSqlite(),
            connectionString: "Data Source=:memory:");

        NotesDbContextForSqlite db = new(options.Options);
        RelationalDatabaseFacadeExtensions.OpenConnection(db.Database);
        db.Database.EnsureCreated();
        return db;
    }

    [method: @Fact]
    public async Task CreateNotePersistsToSQLiteAsync()
    {
        NotesDbContextForSqlite db = CreateDb();
        NoteRepository repo = new(db);
        NoteHandler handler = new(repo);

        Note note = await handler.CreateNoteWithHandlerAsync("Title1", "Body1");

        Assert.NotEqual(Guid.Empty, note.NoteID);
        Assert.Equal("Title1", note.Title);
        Assert.Equal("Body1", note.Body);

        var fromDb = await repo.GetNoteByIdAsync(note.NoteID);
        Assert.Equal("Title1", fromDb.Title);
    }

    [method: @Fact]
    public async Task UpdateNoteChangesPersistInSQLiteAsync()
    {
        NotesDbContextForSqlite db = CreateDb();
        NoteRepository repo = new(db);
        NoteHandler handler = new(repo);

        Note note = await handler.CreateNoteWithHandlerAsync("Old", "OldBody");

        Note updated = await handler.UpdateNoteWithHandlerAsync(
            note.NoteID, "New", "NewBody");

        Assert.Equal("New", updated.Title);
        Assert.Equal("NewBody", updated.Body);

        Note fromDb = await repo.GetNoteByIdAsync(note.NoteID);
        Assert.Equal("New", fromDb.Title);
    }

    [method: @Fact]
    public async Task SearchNotesFindsMatchesAsync()
    {
        NotesDbContextForSqlite db = CreateDb();
        NoteRepository repo = new(db);
        NoteHandler handler = new(repo);

        await handler.CreateNoteWithHandlerAsync("Alpha", "Body");
        await handler.CreateNoteWithHandlerAsync("Beta", "Body");
        await handler.CreateNoteWithHandlerAsync("Gamma", "Body");

        Notes results = await handler.SearchNotesWithHandlerAsync("a");
        Assert.True(System.Linq.Enumerable.Any(results, note => note.Title == "Alpha"));
        Assert.True(System.Linq.Enumerable.Any(results, note => note.Title == "Gamma"));
    }

    [method: @Fact]
    public async Task DeleteNoteRemovesFromSQLiteAsync()
    {
        NotesDbContextForSqlite db = CreateDb();
        NoteRepository repo = new(db);
        NoteHandler handler = new(repo);

        Note note = await handler.CreateNoteWithHandlerAsync("T", "B");

        await handler.DeleteNoteWithHandlerAsync(note.NoteID);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => repo.GetNoteByIdAsync(note.NoteID));
    }
}
#endregion "NotesHandlers_Int/NoteHandlerIntTests.cs"
