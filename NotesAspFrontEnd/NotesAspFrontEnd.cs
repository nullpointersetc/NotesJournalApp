#region "NotesAspFrontEnd.cs"
using Microsoft.AspNetCore.Builder;
using Console = System.Console;
using HttpLoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0130
namespace NullPointersEtc.NotesJournalApp.NotesAspFrontEnd;

public static class NotesAspFrontEnd
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder =
            WebApplication.CreateBuilder(args);

        builder.Services.AddHttpLogging(
            logging => logging.LoggingFields =
                HttpLoggingFields.Request |
                HttpLoggingFields.RequestHeaders |
                HttpLoggingFields.Response |
                HttpLoggingFields.ResponseHeaders |
                HttpLoggingFields.Duration);

        string restApiURL = builder.Configuration["RestApiURL"]
            ?? "http://localhost:17740";

        if (restApiURL != "http://localhost:17740"
            && restApiURL != "https://localhost:17741")
        {
            Console.Error.WriteLine(
                "RestApiURL is not set to a valid URL");

            return;
        }

        string configDotJs = "window.appConfig = { restApiURL: '" +
            restApiURL + "' };";

        WebApplication app = builder.Build();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseHttpLogging();

        app.MapGet("/config.js",
            () => Microsoft.AspNetCore.Http.Results.Content(
                configDotJs, "application/javascript"));

        Console.WriteLine("Serving the front end on " + restApiURL);
        app.Run();
    }
}
#endregion "NotesAspFrontEnd.cs"
