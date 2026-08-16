#region "NotesAspFrontEnd.cs"
using Microsoft.AspNetCore.Builder;
using Console = System.Console;

#pragma warning disable IDE0130
namespace NullPointersEtc.NotesJournalApp.NotesAspFrontEnd;

public static class NotesAspFrontEnd
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder =
            WebApplication.CreateBuilder(args);

        string restApiURL = builder.Configuration["RestApiURL"]
            ?? "http://localhost:5120";

        if (restApiURL != "http://localhost:5120"
            && restApiURL != "https://localhost:5120")
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

        app.MapGet("/config.js",
            () => Microsoft.AspNetCore.Http.Results.Content(
                configDotJs, "application/javascript"));

        Console.WriteLine("Serving the front end on " + restApiURL);
        app.Run();
    }
}
#endregion "NotesAspFrontEnd.cs"
