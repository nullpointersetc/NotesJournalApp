#region "NotesBackEnd/NotesRestAPI.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240, IDE0290
#nullable enable

using ApiController = Microsoft.AspNetCore.Mvc.ApiControllerAttribute;
using ControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;
using DateTime = System.DateTime;
using Enumerable = System.Linq.Enumerable;
using FromQuery = Microsoft.AspNetCore.Mvc.FromQueryAttribute;
using Guid = System.Guid;
using HttpDelete = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGet = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPost = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPut = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using INoteHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.INoteHandler;
using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;

using Notes = System.Collections.Generic.IEnumerable<
    NullPointersEtc.NotesJournalApp.NoteEntity.Note>;

using TaskReturningIActionResult =
    System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult>;

namespace NullPointersEtc.NotesJournalApp.NotesBackEnd;

[type: ApiController, Route("api/notes")]
public sealed class NotesRestAPI : ControllerBase
{
    public NotesRestAPI(INoteHandler handler)
    {
        myHandler = handler;
    }


    [method: HttpPost]
    public async TaskReturningIActionResult HttpPostCreateNoteAsync(
        CreateNoteDTO note)
    {
        Note note2 = await myHandler.CreateNoteWithHandlerAsync(
            title: note.Title, body: note.Body);

        return Ok(new NoteDTO(note2));
    }


    [method: HttpGet("{noteID:guid}")]
    public async TaskReturningIActionResult HttpGetNoteFromNoteIdAsync(
        Guid noteID)
    {
        Note note1 = await myHandler.GetNoteFromNoteIdWithHandlerAsync(
            noteID);

        return Ok(new NoteDTO(note1));
    }


    [method: HttpGet("search")]
    public async TaskReturningIActionResult HttpGetSearchNotesAsync(
        [FromQuery] string query)
    {
        Notes results = await myHandler.SearchNotesWithHandlerAsync(query);

        return Ok(Enumerable.Select<Note, NoteDTO>(
            results, n => new NoteDTO(n)));
    }


    [method: HttpPut("{noteID:guid}")]
    public async TaskReturningIActionResult HttpPutUpdateNoteAsync(
        Guid noteID, UpdateNoteDTO note)
    {
        Note note2 = await myHandler.UpdateNoteWithHandlerAsync(
            noteID: noteID,
            title: note.Title, body: note.Body);

        return Ok(new NoteDTO(note2));
    }


    [HttpDelete("{noteID:guid}")]
    public async TaskReturningIActionResult HttpDeleteNoteAsync(
        Guid noteID)
    {
        await myHandler.DeleteNoteWithHandlerAsync(noteID);
        return NoContent();
    }


    private readonly INoteHandler myHandler;
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
