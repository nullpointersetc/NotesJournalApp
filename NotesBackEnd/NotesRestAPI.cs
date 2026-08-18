#region "NotesBackEnd/NotesRestAPI.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using ApiController = Microsoft.AspNetCore.Mvc.ApiControllerAttribute;
using ControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;
using DateTime = System.DateTime;
using System.Linq;
using FromQuery = Microsoft.AspNetCore.Mvc.FromQueryAttribute;
using Guid = System.Guid;
using HttpDelete = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGet = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPost = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPut = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using INoteHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.INoteHandler;
using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Notes = System.Collections.Generic.IEnumerable<
    NullPointersEtc.NotesJournalApp.NoteEntity.Note>;

using TaskReturningIActionResult =
    System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>;

using TaskReturningIResult = System.Threading.Tasks.Task<
        Microsoft.AspNetCore.Http.IResult>;

namespace NullPointersEtc.NotesJournalApp.NotesBackEnd;

public static class NotesRestAPI
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost(CreateNoteURI, HttpPostCreateNoteAsync)
            .RequireAuthorization()
            .WithTags("Notes")
            .WithSummary("Create a new note")
            .WithDescription("Creates a new note using the provided title and body.")
            .Accepts<CreateNoteDTO>("application/json")
            .Produces<NoteDTO>(StatusCodes.Status200OK);

        app.MapGet(GetAllNotesURI, HttpGetAllNotesAsync)
            .RequireAuthorization()
            .WithTags("Notes")
            .WithSummary("Get all notes")
            .WithDescription("Gets all notes that are currently in the system.")
            .Produces<System.Collections.Generic.IEnumerable<NoteDTO>>(
                StatusCodes.Status200OK);

        app.MapGet(GetNoteURI, HttpGetNoteByNoteIdAsync)
            .RequireAuthorization()
            .WithTags("Notes")
            .WithSummary("Get a note by ID")
            .WithDescription("Retrieves a note using its GUID identifier.")
            .Produces<NoteDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPut(UpdateNoteURI, HttpPutUpdateNoteByNoteIdAsync)
            .RequireAuthorization()
            .WithTags("Notes")
            .WithSummary("Update a note")
            .WithDescription("Updates the title and body of an existing note.")
            .Accepts<UpdateNoteDTO>("application/json")
            .Produces<NoteDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapDelete(DeleteNoteURI, HttpDeleteNoteByNoteIdAsync)
            .RequireAuthorization()
            .WithTags("Notes")
            .WithSummary("Delete a note")
            .WithDescription("Deletes a note using its GUID identifier.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        app.MapGet(SearchNotesURI, HttpGetNotesFromSearchAsync)
            .RequireAuthorization()
            .WithTags("Notes")
            .WithSummary("Search notes")
            .WithDescription("Searches notes by title or body text.")
            .Produces<System.Collections.Generic.IEnumerable<NoteDTO>>(
                StatusCodes.Status200OK);
    }


    private const string CreateNoteURI = "/api/notes";

    public static async TaskReturningIResult
        HttpPostCreateNoteAsync(
            INoteHandler handler, CreateNoteDTO dto)
    {
        Note note = await handler.CreateNoteWithHandlerAsync(
            title: dto.Title, body: dto.Body);

        return Results.Ok(new NoteDTO(note));
    }


    private const string GetAllNotesURI = "/api/notes";

    public static async TaskReturningIResult
        HttpGetAllNotesAsync(INoteHandler handler)
    {
        var notes = await handler.GetAllNotesWithHandlerAsync();
        return Results.Ok(notes.Select<Note, NoteDTO>(n => new NoteDTO(n)));
    }


    private const string GetNoteURI = "/api/notes/{noteID:guid:required}";

    public static async TaskReturningIResult
        HttpGetNoteByNoteIdAsync(
            INoteHandler handler, Guid noteID)
    {
        Note note = await handler.GetNoteFromNoteIdWithHandlerAsync(noteID);
        return Results.Ok(new NoteDTO(note));
    }


    private const string UpdateNoteURI = "/api/notes/{noteID:guid:required}";

    public static async TaskReturningIResult
        HttpPutUpdateNoteByNoteIdAsync(
            INoteHandler handler, Guid noteID, UpdateNoteDTO dto)
    {
        var updated = await handler.UpdateNoteWithHandlerAsync(noteID, dto.Title, dto.Body);
        return Results.Ok(new NoteDTO(updated));
    }

    private const string DeleteNoteURI = "/api/notes/{noteID:guid:required}";

    public static async TaskReturningIResult
        HttpDeleteNoteByNoteIdAsync(
            INoteHandler handler, Guid noteID)
    {
        await handler.DeleteNoteWithHandlerAsync(noteID);
        return Results.NoContent();
    }


    private const string SearchNotesURI = "/api/notes/search";

    public static async TaskReturningIResult
        HttpGetNotesFromSearchAsync(INoteHandler handler, string query)
    {
        var notes = await handler.SearchNotesWithHandlerAsync(query);
        return Results.Ok(notes.Select<Note, NoteDTO>(n => new NoteDTO(n)));
    }
}


public sealed class NoteDTO
{
    public NoteDTO(Note note)
    {
        noteIdField = note.NoteID;
        titleField = note.Title;
        bodyField = note.Body;
        createdAtField = note.CreatedAt;
        lastModifiedAtField = note.LastModifiedAt;
    }

    public Guid NoteID { get => noteIdField; }
    public string Title { get => titleField; }
    public string Body { get => bodyField; }
    public DateTime CreatedAt { get => createdAtField; }
    public DateTime UpdatedAt { get => lastModifiedAtField; }

    private readonly Guid noteIdField;
    private readonly string titleField;
    private readonly string bodyField;
    private readonly DateTime createdAtField;
    private readonly DateTime lastModifiedAtField;
}


public sealed class CreateNoteDTO
{
    public CreateNoteDTO(string title, string body)
    {
        titleField = title;
        bodyField = body;
    }
    public string Title { get => titleField; }
    public string Body { get => bodyField; }

    private readonly string titleField;
    private readonly string bodyField;
}


public sealed class UpdateNoteDTO
{
    public UpdateNoteDTO(string title, string body)
    {
        titleField = title;
        bodyField = body;
    }

    public string Title { get => titleField; }
    public string Body { get => bodyField; }

    private readonly string titleField;
    private readonly string bodyField;
}
#endregion "NotesBackEnd/NotesRestAPI.cs"
