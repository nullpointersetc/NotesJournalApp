#region "NotesApiClient.cs"
#pragma warning disable IDE0290, IDE0301

using NoteDTO = NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.NoteDTO;

using CreateNoteDTO =
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.CreateNoteDTO;

using UpdateNoteDTO =
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.UpdateNoteDTO;

using Task_Nullable_NoteDTO_t = System.Threading.Tasks.Task<
        NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.NoteDTO?>;

using Task_IEnumerable_NoteDTO_t =
    System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.NoteDTO>>;

using IEnumerable_NoteDTO_t =
    System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.NoteDTO>;

using Guid = System.Guid;
using HttpClient = System.Net.Http.HttpClient;
using Task = System.Threading.Tasks.Task;

namespace NullPointersEtc.NotesJournalApp.NotesBlazorFront.Services;

public class NotesApiClient
{
    public NotesApiClient(HttpClient http)
    {
        http1 = http;
    }


    public async Task_IEnumerable_NoteDTO_t GetAllAsync()
    {
        return await http1.GetFromJsonAsync<
            IEnumerable_NoteDTO_t>("api/notes")
                ?? Enumerable.Empty<NoteDTO>();
    }


    public async Task_Nullable_NoteDTO_t GetAsync(Guid noteID)
    {
        return await http1.GetFromJsonAsync<NoteDTO>(
            "api/notes/" + noteID);
    }


    public async Task_IEnumerable_NoteDTO_t
        SearchAsync(string query)
    {
        return await http1.GetFromJsonAsync<
            System.Collections.Generic.IEnumerable<NoteDTO>>(
                "api/notes/search?query=" + Uri.EscapeDataString(query))
                    ?? Enumerable.Empty<NoteDTO>();
    }


    public async Task_Nullable_NoteDTO_t CreateAsync(
        string title, string body)
    {
        CreateNoteDTO payload = new(Title: title, Body: body);

        HttpResponseMessage response =
            await http1.PostAsJsonAsync("api/notes", payload);

        return await response.Content.ReadFromJsonAsync<NoteDTO>();
    }


    public async Task_Nullable_NoteDTO_t
        UpdateAsync(Guid noteID,
            string title, string body)
    {
        UpdateNoteDTO payload = new(Title: title, Body: body);

        HttpResponseMessage response =
            await http1.PutAsJsonAsync("api/notes/" + noteID, payload);

        return await response.Content.ReadFromJsonAsync<NoteDTO>();
    }


    public async Task DeleteAsync(Guid noteID)
    {
        await http1.DeleteAsync("api/notes/" + noteID);
    }

    private readonly HttpClient http1;
}
#endregion "NotesApiClient.cs"
