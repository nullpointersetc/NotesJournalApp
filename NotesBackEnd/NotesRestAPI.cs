#region NotesRestAPI classes
#pragma warning disable IDE0001, IDE0290
#nullable enable

using Microsoft.AspNetCore.Mvc;
using NullPointersEtc.NotesJournalApp.NotesHandlers;

using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;

namespace NullPointersEtc.NotesJournalApp.NotesRestApiTypes;

[type: ApiController, Route("api/notes")]
public class NotesRestAPI : ControllerBase
{
    public NotesRestAPI(INoteHandler handler)
    {
        handler1 = handler;
    }

    [method: HttpPost]
    public async Task<IActionResult> Create(
        CreateNoteDTO note)
    {
        Note note2 = await handler1.CreateAsync(
            title: note.Title, body: note.Body);

        return Ok(new NoteDTO(note2));
    }

    [method: HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        Note note1 = await handler1.GetNotesAsync(id);
        return Ok(new NoteDTO(note1));
    }

    [method: HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string query)
    {
        System.Collections.Generic.IEnumerable<Note>
            results = await handler1.SearchAsync(query);

        return Ok(results.Select(n => new NoteDTO(n)));
    }

    [method: HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id, UpdateNoteDTO note)
    {
        Note note2 = await handler1.GetNotesAsync(id);

        Note note3 = await handler1.UpdateAsync(
            noteID: note2.NoteID, title: note.Title, body: note.Body);

        return Ok(new NoteDTO(note3));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await handler1.DeleteAsync(id);
        return NoContent();
    }

    private readonly INoteHandler handler1;
}


public class NoteDTO
{
    public NoteDTO(Note note)
    {
        id1 = note.NoteID;
        title1 = note.Title;
        body1 = note.Body;
        created1 = note.CreatedAt;
        modified1 = note.LastUpdatedAt;
    }

    public Guid Id { get => id1; }
    public string Title { get => title1; }
    public string Body { get => body1; }
    public DateTime CreatedAt { get => created1; }
    public DateTime UpdatedAt { get => modified1; }

    private readonly Guid id1;
    private readonly string title1;
    private readonly string body1;
    private readonly DateTime created1;
    private readonly DateTime modified1;
}


public class CreateNoteDTO
{
    public CreateNoteDTO(string title, string body)
    {
        title1 = title;
        body1 = body;
    }
    public string Title { get => title1; }
    public string Body { get => body1; }
    private readonly string title1;
    private readonly string body1;
}


public class UpdateNoteDTO
{
    public UpdateNoteDTO(string title, string body)
    {
        title1 = title;
        body1 = body;
    }

    public string Title { get => title1; }
    public string Body { get => body1; }

    private readonly string title1;
    private readonly string body1;
}
#endregion NotesRestAPI classes
