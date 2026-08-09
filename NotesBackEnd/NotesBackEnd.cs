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

using ServiceCollectionServiceExtensions =
    Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions;

using OpenApiServiceCollectionExtensions =
    Microsoft.Extensions.DependencyInjection.OpenApiServiceCollectionExtensions;

using EntityFrameworkServiceCollectionExtensions =
    Microsoft.Extensions.DependencyInjection.EntityFrameworkServiceCollectionExtensions;

using SqlServerDbContextOptionsExtensions =
    Microsoft.EntityFrameworkCore.SqlServerDbContextOptionsExtensions;

using SqliteDbContextOptionsBuilderExtensions =
    Microsoft.EntityFrameworkCore.SqliteDbContextOptionsBuilderExtensions;

using HostEnvironmentEnvExtensions =
    Microsoft.Extensions.Hosting.HostEnvironmentEnvExtensions;

using OpenApiEndpointRouteBuilderExtensions =
    Microsoft.AspNetCore.Builder.OpenApiEndpointRouteBuilderExtensions;

using HttpsPolicyBuilderExtensions =
    Microsoft.AspNetCore.Builder.HttpsPolicyBuilderExtensions;

using ConfigurationExtensions =
    Microsoft.Extensions.Configuration.ConfigurationExtensions;

using Console = System.Console;
using Enumerable = System.Linq.Enumerable;
using StringComparison = System.StringComparison;

namespace NullPointersEtc.NotesJournalApp.NotesBackEnd;

public class NotesBackEnd
{
    private static void Main(string[] args)
    {
        string dbSqlServer = "--db=SqlServer";

        bool useSqlServer = Enumerable.Any(args,
            arg => arg.Equals(dbSqlServer,
                StringComparison.OrdinalIgnoreCase));

        string dbSQLite = "--db=SQLite";

        bool useSQLite = Enumerable.Any(args,
            arg => arg.Equals(dbSQLite,
            StringComparison.OrdinalIgnoreCase));

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

        ServiceCollectionServiceExtensions.AddScoped<
            INoteHandler, NoteHandler>(builder.Services);

        ServiceCollectionServiceExtensions.AddScoped<
            IUserHandler, UserHandler>(builder.Services);

        ServiceCollectionServiceExtensions.AddScoped<
            INoteRepository, NoteRepository>(builder.Services);

        ServiceCollectionServiceExtensions.AddScoped<
            IUserRepository, UserRepository>(builder.Services);

        OpenApiServiceCollectionExtensions.AddOpenApi(builder.Services);

        if (useSqlServer)
            EntityFrameworkServiceCollectionExtensions.AddDbContext<
                NotesDbContextForSqlServer>(builder.Services,
                    options => SqlServerDbContextOptionsExtensions.UseSqlServer(
                        options,
                        ConfigurationExtensions.GetConnectionString(
                            builder.Configuration, "AzureSql")));

        if (useSQLite)
            EntityFrameworkServiceCollectionExtensions.AddDbContext<
                NotesDbContextForSqlite>(builder.Services,
                options => SqliteDbContextOptionsBuilderExtensions.UseSqlite(
                    options,
                    ConfigurationExtensions.GetConnectionString(
                        builder.Configuration, "NotesDb")));

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (HostEnvironmentEnvExtensions.IsDevelopment(app.Environment))
            OpenApiEndpointRouteBuilderExtensions.MapOpenApi(app);

        HttpsPolicyBuilderExtensions.UseHttpsRedirection(app);
        app.Run();
    }
}

#endregion "NotesBackEnd/NotesBackEnd.cs"
