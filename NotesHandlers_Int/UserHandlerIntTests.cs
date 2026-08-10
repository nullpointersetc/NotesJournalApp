﻿#region "NotesHandlers_Int/UserHandlerIntTests.cs"
#pragma warning disable IDE0130

using Assert = Xunit.Assert;
using DbUpdateException = Microsoft.EntityFrameworkCore.DbUpdateException;
using Fact = Xunit.FactAttribute;
using RelationalDatabaseFacadeExtensions = Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions;
using NotesDbContextForSqlite = NullPointersEtc.NotesJournalApp.NotesStorage.NotesDbContextForSqlite;
using UserRepository = NullPointersEtc.NotesJournalApp.NotesStorage.UserRepository;
using UserHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.UserHandler;
using User = NullPointersEtc.NotesJournalApp.UserEntity.User;
using System;
using Task = System.Threading.Tasks.Task;

using DbContextOptionsBuilderNotesDbContextForSqlite =
    Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<
        NullPointersEtc.NotesJournalApp.NotesStorage.NotesDbContextForSqlite>;

using SqliteDbContextOptionsBuilderExtensions =
    Microsoft.EntityFrameworkCore.SqliteDbContextOptionsBuilderExtensions;

namespace NullPointersEtc.NotesJournalApp.NotesHandlers_IntegrationTests;

public class UserHandlerIntegrationTests
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
    public async Task CreateUserPersistsToSQLiteAsync()
    {
        NotesDbContextForSqlite db = CreateDb();
        UserRepository repo = new(db);
        UserHandler handler = new(repo);

        User user = await handler.CreateUserWithHandlerAsync(
            "user1", "User One", "user1@mail.com");

        Assert.Equal("user1", user.UserName);

        User actual = await repo.GetUserByUserIdAsync(user.UserID);
        Assert.Equal("User One", actual.DisplayName);
    }


    [method: @Fact]
    public async Task UpdateUserPersistsChangesAsync()
    {
        NotesDbContextForSqlite db = CreateDb();
        UserRepository repo = new(db);
        UserHandler handler = new(repo);

        User user = await handler.CreateUserWithHandlerAsync(
            "user1", "OldName", "old@mail.com");

        User updated = await handler.UpdateUserWithHandlerAsync(
            user.UserID, "NewName", "new@mail.com");

        Assert.Equal("NewName", updated.DisplayName);

        User actual = await repo.GetUserByUserIdAsync(user.UserID);
        Assert.Equal("NewName", actual.DisplayName);
    }


    [method: @Fact]
    public async Task UniqueUserNameConstraintIsEnforcedAsync()
    {
        NotesDbContextForSqlite db = CreateDb();
        UserRepository repo = new(db);
        UserHandler handler = new(repo);

        await handler.CreateUserWithHandlerAsync(
            "user1", "User One", "one@mail.com");

        await Assert.ThrowsAsync<DbUpdateException>(
            () => handler.CreateUserWithHandlerAsync(
                "user1", "Duplicate", "dup@mail.com"));
    }

    [method: @Fact]
    public async Task DeleteUserRemovesFromSQLiteAsync()
    {
        NotesDbContextForSqlite db = CreateDb();
        UserRepository repo = new(db);
        UserHandler handler = new(repo);

        var user = await handler.CreateUserWithHandlerAsync(
            "user1", "User One", "one@mail.com");

        await handler.DeleteWithHandlerAsync(user.UserID);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => repo.GetUserByUserIdAsync(user.UserID));
    }
}
#endregion "NotesHandlers_Int/UserHandlerIntTests.cs"
