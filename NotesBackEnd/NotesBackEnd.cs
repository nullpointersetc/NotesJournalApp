#region "NotesBackEnd/NotesBackEnd.cs"

#pragma warning disable IDE0001, IDE0002, IDE0130, IDE0240
#nullable enable

using NullPointersEtc.NotesJournalApp.NotesHandlers;
using NullPointersEtc.NotesJournalApp.NotesStorage;
using NullPointersEtc.NotesJournalApp.NoteEntity;
using NullPointersEtc.NotesJournalApp.UserEntity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Console = System.Console;
using StringComparison = System.StringComparison;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

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
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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
        NotesRestAPI.MapEndpoints(app);
        UsersRestAPI.MapEndpoints(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Run();
    }
}

#endregion "NotesBackEnd/NotesBackEnd.cs"
