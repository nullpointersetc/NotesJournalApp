#region "NotesBackEnd/NotesBackEnd.cs"

#pragma warning disable IDE0001, IDE0002, IDE0130, IDE0240
#pragma warning disable IDE0350
#nullable enable

using INoteHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.INoteHandler;
using IUserHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.IUserHandler;

using NoteHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.NoteHandler;
using UserHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.UserHandler;

using INoteRepository = NullPointersEtc.NotesJournalApp.NoteEntity.INoteRepository;
using IUserRepository = NullPointersEtc.NotesJournalApp.UserEntity.IUserRepository;

using NoteRepository = NullPointersEtc.NotesJournalApp.NotesStorage.NoteRepository;
using UserRepository = NullPointersEtc.NotesJournalApp.NotesStorage.UserRepository;

using WebApplication =
    Microsoft.AspNetCore.Builder.WebApplication;

using WebApplicationBuilder =
    Microsoft.AspNetCore.Builder.WebApplicationBuilder;

using NotesDbContextForSqlite =
    NullPointersEtc.NotesJournalApp.NotesStorage.NotesDbContextForSqlite;

using NotesDbContextForSqlServer =
    NullPointersEtc.NotesJournalApp.NotesStorage.NotesDbContextForSqlServer;








using Console = System.Console;
using StringComparison = System.StringComparison;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;

namespace NullPointersEtc.NotesJournalApp.NotesBackEnd;

public class NotesBackEnd
{
    private static void Main(string[] args)
    {
        const string dbSqlServer = "--db=SqlServer",
            dbSQLite = "--db=SQLite";

        bool useSqlServer = args.Any(arg => arg.Equals(
                dbSqlServer, StringComparison.OrdinalIgnoreCase)),
            useSQLite = args.Any(arg => arg.Equals(
                dbSQLite, StringComparison.OrdinalIgnoreCase));

        if (!useSqlServer && !useSQLite
            || useSqlServer && useSQLite)
        {
            Console.WriteLine("NotesBackEnd: must use one of " +
                dbSqlServer + " or " + dbSQLite);

            return;
        }

        WebApplicationBuilder builder =
            WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<INoteHandler, NoteHandler>();
        builder.Services.AddScoped<IUserHandler, UserHandler>();
        builder.Services.AddScoped<INoteRepository, NoteRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        string conn = builder.Configuration.GetConnectionString("NotesDb")
            ?? throw new System.InvalidOperationException(
                "Connection string \"NotesDb\" not found");

        if (useSqlServer)
            builder.Services.AddDbContext<NotesDbContextForSqlServer>(
                optionsBuilder => optionsBuilder.UseSqlServer(conn));

        if (useSQLite)
            builder.Services.AddDbContext<NotesDbContextForSqlite>(
                optionsBuilder => optionsBuilder.UseSqlite(conn));

        WebApplication app = builder.Build();
        app.UseHttpsRedirection();
        app.Run();
    }
}

#endregion "NotesBackEnd/NotesBackEnd.cs"
