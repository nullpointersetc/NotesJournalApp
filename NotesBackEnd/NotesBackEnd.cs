#region class NotesBackEnd - The mainline class

#pragma warning disable IDE0001, IDE0002, IDE0130, IDE0240
#nullable enable

using INoteHandler = NullPointersEtc.NotesJournalApp.NoteHandler.INoteHandler;
using IUserHandler = NullPointersEtc.NotesJournalApp.UserHandler.IUserHandler;

using NoteHandler_t = NullPointersEtc.NotesJournalApp.NoteHandler.NoteHandler;
using UserHandler_t = NullPointersEtc.NotesJournalApp.UserHandler.UserHandler;

using INoteRepository = NullPointersEtc.NotesJournalApp.NoteEntity.INoteRepository;
using IUserRepository = NullPointersEtc.NotesJournalApp.UserEntity.IUserRepository;

using NoteRepository = NullPointersEtc.NotesJournalApp.NotesStorage.NoteRepository;
using UserRepository = NullPointersEtc.NotesJournalApp.NotesStorage.UserRepository;

using WebApplication =
    Microsoft.AspNetCore.Builder.WebApplication;

using WebApplicationBuilder =
    Microsoft.AspNetCore.Builder.WebApplicationBuilder;

using NotesDbContext =
    NullPointersEtc.NotesJournalApp.NotesStorage.NotesDbContext;

using NotesDbContextParams =
    NullPointersEtc.NotesJournalApp.NotesStorage.NotesDbContextParams;

using Microsoft.EntityFrameworkCore;

namespace NullPointersEtc.NotesJournalApp.NotesBackEnd;

public class NotesBackEnd
{
    private static void Main(string[] args)
    {
        string dbSqlServer = "--db=SqlServer";

        bool useSqlServer = args.Any(
            arg => arg.Equals(dbSqlServer,
            StringComparison.OrdinalIgnoreCase));

        string dbSQLite = "--db=SQLite";

        bool useSQLite = true/*args.Any(
            arg => arg.Equals(dbSQLite,
            StringComparison.OrdinalIgnoreCase))*/;

        if (!useSqlServer && !useSQLite
            || useSqlServer && useSQLite)
        {
            Console.Write("NotesBackEnd: must use one of ");
            Console.Write(dbSqlServer);
            Console.Write(" or ");
            Console.WriteLine(dbSQLite);
            return;
        }

        WebApplicationBuilder builder =
            WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<INoteHandler, NoteHandler_t>();
        builder.Services.AddScoped<IUserHandler, UserHandler_t>();
        builder.Services.AddScoped<INoteRepository, NoteRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddOpenApi();

        if (useSqlServer)
            builder.Services.AddSingleton(
                new NotesDbContextParams(useSqlServer: true))
                .AddDbContext<NotesDbContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("AzureSql")));

        if (useSQLite)
            builder.Services.AddSingleton(
                new NotesDbContextParams(useSqlServer: false))
                .AddDbContext<NotesDbContext>(
                options => options.UseSqlite(
                    builder.Configuration.GetConnectionString("NotesDb")));

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.Run();
    }
}

#endregion class NotesBackEnd
