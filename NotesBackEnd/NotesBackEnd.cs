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
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using Dictionary_type =
    System.Collections.Generic.Dictionary<string,
    NullPointersEtc.NotesJournalApp.NotesBackEnd.ConnectionConfig>;

namespace NullPointersEtc.NotesJournalApp.NotesBackEnd;

public class NotesBackEnd
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder =
            WebApplication.CreateBuilder(args);

        Dictionary_type? allConnections =
            builder.Configuration.GetSection("Database")
                .Get<Dictionary_type>();

        if (allConnections is null || allConnections.Count == 0)
        {
            Console.WriteLine("Database is absent or empty in appsettings.json.");
            return;
        }

        ConnectionConfig? connectionConfig = null;

        string? matchingArg = args.FirstOrDefault(
            arg => allConnections.TryGetValue(arg, out connectionConfig));

        if (connectionConfig is not null)
        {
            if (matchingArg is not null)
            {
                Console.Write("Configuration selected: ");
                Console.WriteLine(matchingArg);
                Console.Write("Type: ");
                Console.WriteLine(connectionConfig.Type);
                Console.Write("Connection String: ");
                Console.WriteLine(connectionConfig.ConnectionString);
            }
            else
            {
                Console.Error.WriteLine("Internal error: connection name has no config");
                return;
            }
        }
        else if (allConnections.TryGetValue("default", out connectionConfig))
        {
            Console.WriteLine("Using default configuration.");
            Console.Write("Type: ");
            Console.WriteLine(connectionConfig.Type);
            Console.Write("Connection String: ");
            Console.WriteLine(connectionConfig.ConnectionString);
        }
        else
        {
            Console.Error.WriteLine("No configuration was selected and no default was configured.");
            return;
        }

        builder.Services.AddScoped<INoteHandler, NoteHandler>();
        builder.Services.AddScoped<IUserHandler, UserHandler>();
        builder.Services.AddScoped<INoteRepository, NoteRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        if (connectionConfig.IsSqlServer())
        {
            builder.Services.AddDbContext<NotesDbContext>(
                options => options.UseSqlServer(connectionConfig.ConnectionString))
                    .AddSingleton(MoreOptionsForNotesDbContext.ForSqlServer());
        }
        else if (connectionConfig.IsSqlite())
        {
            builder.Services.AddDbContext<NotesDbContext>(
                options => options.UseSqlite(connectionConfig.ConnectionString))
                    .AddSingleton(MoreOptionsForNotesDbContext.ForSqlite());
        }
        else
        {
            Console.WriteLine("Unknown database type: " + connectionConfig.Type);
        }

        WebApplication app = builder.Build();
        NotesRestAPI.MapEndpoints(app);
        UsersRestAPI.MapEndpoints(app);

        using (IServiceScope scope = app.Services.CreateScope())
        {
            using NotesDbContext db =
                scope.ServiceProvider.GetRequiredService<NotesDbContext>();

            if (db.Database.IsSqlite())
                db.Database.EnsureCreated();

            if (db.Database.IsSqlServer())
                db.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Run();
    }
}

public sealed class NotesBackEndOptions
{
    public NotesBackEndOptions()
    {
        databases = new(System.StringComparer.OrdinalIgnoreCase);
    }

    public Dictionary_type Database
    {
        get => databases;
        set => databases = value;
    }

    private Dictionary_type databases;
}

public sealed class ConnectionConfig
{
    public ConnectionConfig()
    {
        isSqlServer = false;
        isSqlite = false;
        connString = null;
    }

    public string Type
    {
        get
        {
            if (isSqlServer)
                return "SqlServer";
            else if (isSqlite)
                return "Sqlite";
            else
                throw new System.InvalidOperationException("Type not set");
        }

        set
        {
            if (value == "SqlServer")
            {
                isSqlServer = true;
                isSqlite = false;
            }
            else if (value == "Sqlite")
            {
                isSqlServer = false;
                isSqlite = true;
            }
            else
            {
                throw new System.ArgumentException("Invalid Type");
            }
        }
    }

    public string ConnectionString
    {
        get => connString ??
            throw new System.InvalidOperationException("ConnectionString not set");

        set => connString = value;
    }

    public bool IsSqlServer() => isSqlServer;
    public bool IsSqlite() => isSqlite;

    private bool isSqlServer, isSqlite;
    private string? connString;
}


#endregion "NotesBackEnd/NotesBackEnd.cs"
