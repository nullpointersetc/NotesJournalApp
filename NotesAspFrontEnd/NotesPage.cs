#region "NotesAspFrontEnd.cs"
using DateTime = System.DateTime;
using Guid = System.Guid;
using HttpStatusCode = System.Net.HttpStatusCode;
using InvalidOperationException = System.InvalidOperationException;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;

using AuthenticationHeaderValue =
    System.Net.Http.Headers.AuthenticationHeaderValue;

using List_of_NoteResponse =
    System.Collections.Generic.List<
        NullPointersEtc.NotesJournalApp.NotesAspFrontEnd.NoteResponse>;

using Task_returning_IResult =
    System.Threading.Tasks.Task<Microsoft.AspNetCore.Http.IResult>;

#pragma warning disable IDE0130
namespace NullPointersEtc.NotesJournalApp.NotesAspFrontEnd;

public static class NotesPage
{
    public static string NotesPageURL
    {
        get => "/notes";
    }


    public static async Task_returning_IResult HttpGetNotes(
        HttpContext context,
        NotesSessions sessions,
        HttpClient client)
    {
        try
        {
            Guid? sessionId = SessionIDs.GetSessionID(context);

            if (sessionId is null
                || !sessions.TryGet(sessionId.Value, out string? token))
                return Results.Redirect("/");

            using HttpRequestMessage request =
                new(HttpMethod.Get, "/api/notes");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                sessions.Remove(sessionId.Value);
                context.Response.Cookies.Delete(CookieNames.SessionCookieName);
                return Results.Redirect("/");
            }

            if (!response.IsSuccessStatusCode)
                return Results.Problem("The notes backend could not return the notes.");

            List_of_NoteResponse? notes =
                await response.Content.ReadFromJsonAsync<
                    List_of_NoteResponse>();

            return NotesPageResult(notes);
        }
        catch (HttpRequestException)
        {
            return Results.Problem("The notes backend is unavailable.");
        }
    }


    private static IResult NotesPageResult(List_of_NoteResponse? notes)
    {
        string content = "<!DOCTYPE html>" +
            "<html lang=\"en\">" +
            "<head><meta charset=\"utf-8\">" +
            "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">" +
            "<title>Notes Journal</title>" +
            Stylesheet.PageStyles() +
            "</head>" +
            "<body>" +
            "<div class=\"eyebrow\">Notes Journal</div>" +
            "<h1>Your notes</h1>" +
            "<form method=\"post\" action=\"/logout\">" +
            "<button class=\"secondary\" type=\"submit\">Log out</button>" +
            "</form>" +
            "<table><thead><tr><th>Title</th><th>Body</th><th>Created</th><th>Updated</th></tr></thead><tbody>";

        if (notes is null)
        {
            content += "<tr><td colspan=\"4\" class=\"empty\">No notes yet.</td></tr>";
        }
        else if (notes.Count == 0)
        {
            content += "<tr><td colspan=\"4\" class=\"empty\">No notes yet.</td></tr>";
        }
        else
        {
            foreach (NoteResponse note in notes)
            {
                content += "<tr><td>" +
                HtmlEncoder.Default.Encode(note.Title) +
                "</td><td>" +
                HtmlEncoder.Default.Encode(note.Body) +
                "</td><td>" +
                note.CreatedAt.ToShortDateString() + " " +
                note.CreatedAt.ToShortTimeString() +
                "</td><td>" +
                note.UpdatedAt.ToShortDateString() + " " +
                note.UpdatedAt.ToShortTimeString() +
                "</td></tr>";
            }
        }

        content += "</tbody></table></main></body></html>";

        return Results.Content(content, "text/html", System.Text.Encoding.ASCII);
    }
}


public sealed class NoteResponse
{
    public NoteResponse()
    {
        t1 = null;
        b1 = null;
        c1 = null;
        u1 = null;
    }

    public string Title
    {
        get => t1 ??
            throw new InvalidOperationException(
                nameof(Title) + " is null");

        set => t1 = value;
    }

    public string Body
    {
        get => b1 ??
            throw new InvalidOperationException(
                nameof(Body) + " is null");

        set => b1 = value;
    }

    public DateTime CreatedAt
    {
        get => c1 ??
            throw new InvalidOperationException(
                nameof(CreatedAt) + " is null");

        set => c1 = value;
    }

    public DateTime UpdatedAt
    {
        get => u1 ??
            throw new InvalidOperationException(
                nameof(UpdatedAt) + " is null");

        set => u1 = value;
    }

    private string? t1;
    private string? b1;
    private DateTime? c1;
    private DateTime? u1;
}
#endregion "NotesAspFrontEnd.cs"
