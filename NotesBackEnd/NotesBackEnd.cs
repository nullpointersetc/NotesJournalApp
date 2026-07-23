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
using Microsoft.EntityFrameworkCore;

namespace NullPointersEtc.NotesJournalApp.NotesBackEnd;

public class NotesBackEnd
{
    private static void Main(string[] args)
    {
        bool sqlServerArg = args.Any(
            arg => arg.Equals("--db=sqlserver",
            StringComparison.OrdinalIgnoreCase));

        bool sqliteArg = args.Any(
            arg => arg.Equals("--db=sqlite",
            StringComparison.OrdinalIgnoreCase));

        if (!sqlServerArg && !sqliteArg
            || sqlServerArg && sqliteArg)
        {
            Console.WriteLine("NotesBackEnd: must have one of --db=sqlserver or --db-sqlite");
            return;
        }

        WebApplicationBuilder builder =
            WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<INoteHandler, NoteHandler_t>();
        builder.Services.AddScoped<IUserHandler, UserHandler_t>();
        builder.Services.AddScoped<INoteRepository, NoteRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddOpenApi();

        if (sqlServerArg)
            builder.Services.AddDbContext<NotesDbContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("AzureSql")));

        if (sqliteArg)
            builder.Services.AddDbContext<NotesDbContext>(
                options => options.UseSqlite(
                    "Data Source=notes.db"));

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        var summaries = new[]
        {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

        app.MapGet("/weatherforecast", () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast");

        app.Run();
    }
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

#endregion class NotesBackEnd
