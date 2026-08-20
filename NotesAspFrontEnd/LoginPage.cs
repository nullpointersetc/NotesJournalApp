#region "NotesAspFrontEnd/LoginPage.cs"
using Guid = System.Guid;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;

using Map_JWT_from_SessionID =
    System.Collections.Concurrent.ConcurrentDictionary<
    System.Guid, string>;

using Task_returning_IResult =
    System.Threading.Tasks.Task<Microsoft.AspNetCore.Http.IResult>;

#pragma warning disable IDE0130
namespace NullPointersEtc.NotesJournalApp.NotesAspFrontEnd;

public static class LoginPage
{
    public static string LoginPageURL
    {
        get => "/login";
    }


    public static IResult LoginPageWithoutError()
        => Results.Content(content: "<!DOCTYPE html>" +
            "<html lang=\"en\">" +
            "<head>" +
            "<meta charset=\"utf-8\">" +
            "<meta name=\"viewport\" content=\"width=device-width, initial-scale=\">" +
            "<title>Notes Journal | Login</title>" +
            Stylesheet.PageStyles() +
            "</head>" +
            "<body>" +
            "<main class=\"login-shell\">" +
            "<section class=\"login-panel\">" +
            "<p class=\"eyebrow\">Notes Journal</p>" +
            "<h1>Welcome back.</h1>" +
            "<p class=\"muted\">Sign in to read your notes.</p>" +
            "<form method=\"post\" action=\"/login\">" +
            "<label for=\"username\">Username</label>" +
            "<input id=\"username\" name=\"username\" type=\"text\" autocomplete=\"username\" required autofocus>" +
            "<label for=\"password\">Password</label>" +
            "<input id=\"password\" name=\"password\" type=\"password\" autocomplete=\"current-password\" required>" +
            "<button type=\"submit\">Sign in</button></form></section></main></body></html>",
            contentType: "text/html",
            contentEncoding: System.Text.Encoding.ASCII);

    public static IResult LoginPageWithError(string error)
        => Results.Content(content: "<!DOCTYPE html>" +
            "<html lang=\"en\">" +
            "<head>" +
            "<meta charset=\"utf-8\">" +
            "<meta name=\"viewport\" content=\"width=device-width, initial-scale=\">" +
            "<title>Notes Journal | Login</title>" +
            Stylesheet.PageStyles() +
            "</head>" +
            "<body>" +
            "<main class=\"login-shell\">" +
            "<section class=\"login-panel\">" +
            "<p class=\"eyebrow\">Notes Journal</p>" +
            "<h1>Welcome back.</h1>" +
            "<p class=\"muted\">Sign in to read your notes.</p>" +
            "<p class=\"error\">" + HtmlEncoder.Default.Encode(error) + "</p>" +
            "<form method=\"post\" action=\"/login\">" +
            "<label for=\"username\">Username</label>" +
            "<input id=\"username\" name=\"username\" type=\"text\" autocomplete=\"username\" required autofocus>" +
            "<label for=\"password\">Password</label>" +
            "<input id=\"password\" name=\"password\" type=\"password\" autocomplete=\"current-password\" required>" +
            "<button type=\"submit\">Sign in</button></form></section></main></body></html>",
            contentType: "text/html",
            contentEncoding: System.Text.Encoding.ASCII);

    public static async Task_returning_IResult HttpPostLogin(
        HttpContext context,
        NotesSessions sessions,
        HttpClient client)
    {
        try
        {
            IFormCollection form = await context.Request.ReadFormAsync();

            HttpResponseMessage response =
                await client.PostAsJsonAsync(
                    requestUri: "/auth/login",
                    value: new LoginDTO(
                        userName: form["username"].ToString(),
                        password: form["password"].ToString()));

            if (!response.IsSuccessStatusCode)
                return LoginPageWithError("Invalid");

            LoginResponseDTO? login =
                await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

            if (login is null || string.IsNullOrWhiteSpace(login.token))
                return LoginPageWithError("Login not formed");

            Guid sessionID = sessions.Create(login.token);

            context.Response.Cookies.Append(CookieNames.SessionCookieName,
                    value: sessionID.ToString(),
                    options: new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Lax,
                        IsEssential = true,
                        MaxAge = System.TimeSpan.FromHours(1)
                    });

            return await NotesPage.HttpGetNotes(
                context, sessions, client);
        }
        catch (HttpRequestException)
        {
            return LoginPageWithError("Notes backend is unavailable");
        }
    }
}


public sealed class NotesSessions
{
    public NotesSessions()
    {
        myLiveSessions = new();
    }

    public Guid Create(string token)
    {
        Guid sessionID = Guid.NewGuid();
        myLiveSessions[sessionID] = token;
        return sessionID;
    }

    public bool Contains(Guid sessionID)
        => myLiveSessions.ContainsKey(sessionID);

    public bool TryGet(Guid sessionID, out string? token)
        => myLiveSessions.TryGetValue(sessionID, out token);

    public void Remove(Guid sessionID)
        => myLiveSessions.TryRemove(sessionID, out _);

    private readonly Map_JWT_from_SessionID myLiveSessions;
}


public static class CookieNames
{
    public static string SessionCookieName
    {
        get => "notes_session";
    }
}


public static class SessionIDs
{
    public static Guid? GetSessionID(
        HttpContext context)
        => Guid.TryParse(
            context.Request.Cookies[CookieNames.SessionCookieName],
            out Guid sessionID)
            ? sessionID : null;
}


public sealed class LoginDTO
{
#pragma warning disable IDE0290
    public LoginDTO(
        string userName,
        string password)
    {
        u1 = userName;
        p1 = password;
    }
#pragma warning restore IDE0290

#pragma warning disable IDE1006
    public string userName
    {
        get => u1;
    }

    public string password
    {
        get => p1;
    }
#pragma warning restore IDE1006

    private readonly string u1, p1;
}

public sealed class LoginResponseDTO
{
#pragma warning disable IDE0290
    public LoginResponseDTO(string token)
    {
        t1 = token;
    }
#pragma warning restore IDE0290

#pragma warning disable IDE1006
    public string token
    {
        get => t1;
    }
#pragma warning restore IDE1006

    private readonly string t1;
}
#endregion "NotesAspFrontEnd/LoginPage.cs"
