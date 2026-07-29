#region "NotesApiClient.cs"
#pragma warning disable IDE0290, IDE0301

using System.Net.Http.Json;

using NoteDTO = NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.NoteDTO;

using CreateNoteDTO =
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.CreateNoteDTO;

using UpdateNoteDTO =
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.UpdateNoteDTO;

namespace NullPointersEtc.NotesJournalApp.NotesBlazorFront.Services;

public class NotesApiClient
{
    public NotesApiClient(HttpClient http)
    {
        http1 = http;
    }

    public async System.Threading.Tasks.Task<
        System.Collections.Generic.IEnumerable<NoteDTO>> GetAllAsync()
    {
        return await http1.GetFromJsonAsync<
            System.Collections.Generic.IEnumerable<NoteDTO>>(
                "api/notes") ?? Enumerable.Empty<NoteDTO>();
    }

    public async System.Threading.Tasks.Task<
        NoteDTO?> GetAsync(System.Guid noteID)
    {
        return await http1.GetFromJsonAsync<NoteDTO>(
            "api/notes/" + noteID);
    }


    public async System.Threading.Tasks.Task<
        System.Collections.Generic.IEnumerable<NoteDTO>>
        SearchAsync(string query)
    {
        return await http1.GetFromJsonAsync<
            System.Collections.Generic.IEnumerable<NoteDTO>>(
                "api/notes/search?query=" + Uri.EscapeDataString(query))
                    ?? Enumerable.Empty<NoteDTO>();
    }


    public async System.Threading.Tasks.Task<
        NoteDTO?> CreateAsync(string title, string body)
    {
        CreateNoteDTO payload = new(Title: title, Body: body);

        HttpResponseMessage response =
            await http1.PostAsJsonAsync("api/notes", payload);

        return await response.Content.ReadFromJsonAsync<NoteDTO>();
    }

    public async System.Threading.Tasks.Task<NoteDTO?>
        UpdateAsync(System.Guid noteID,
            string title, string body)
    {
        UpdateNoteDTO payload = new(Title: title, Body: body);

        HttpResponseMessage response =
            await http1.PutAsJsonAsync("api/notes/" + noteID, payload);

        return await response.Content.ReadFromJsonAsync<NoteDTO>();
    }

    public async System.Threading.Tasks.Task
        DeleteAsync(System.Guid noteID)
    {
        await http1.DeleteAsync("api/notes/"+noteID);
    }

    private readonly HttpClient http1;
}
#endregion "NotesApiClient.cs"
