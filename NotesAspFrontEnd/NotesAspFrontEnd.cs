#region "NotesAspFrontEnd.cs"
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using HttpLoggingFields =
    Microsoft.AspNetCore.HttpLogging.HttpLoggingFields;

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

        builder.Services.AddSingleton(new System.Net.Http.HttpClient()
        {
            BaseAddress = new System.Uri(restApiURL)
        });

        builder.Services.AddSingleton<NotesSessions>();

        WebApplication app = builder.Build();
        app.UseHttpLogging();

        app.Logger.LogInformation(
            "REST API is assumed to be at {restApiURL}", restApiURL);

        app.MapGet("/", NotesAspFrontEnd.HttpGetDefault);

        app.MapPost(LoginPage.LoginPageURL, LoginPage.HttpPostLogin);

        app.MapGet(NotesPage.NotesPageURL, NotesPage.HttpGetNotes);

        app.MapPost("/logout", NotesAspFrontEnd.HttpPostLogout);

        app.Run();
    }


    private static IResult HttpGetDefault(
        HttpContext context, NotesSessions sessions)
    {
        Guid? sessionID = SessionIDs.GetSessionID(context);

        if (sessionID.HasValue && sessions.Contains(sessionID.Value))
            return Results.Redirect(NotesPage.NotesPageURL);
        else
            return LoginPage.LoginPageWithoutError();
    }


    private static IResult HttpPostLogout(
        HttpContext context,
        NotesSessions sessions)
    {
        Guid? sessionID = SessionIDs.GetSessionID(context);

        if (sessionID is not null)
            sessions.Remove(sessionID.Value);

        context.Response.Cookies.Delete(CookieNames.SessionCookieName);

        return Results.Redirect("/");
    }
}
#endregion "NotesAspFrontEnd.cs"
