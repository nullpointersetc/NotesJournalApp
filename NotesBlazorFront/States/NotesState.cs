#region "NotesState.cs"
#pragma warning disable IDE0028, IDE0290, IDE0305

using NoteDTO =
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.NoteDTO;

using List_NoteDTO_t =
    System.Collections.Generic.List<
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.NoteDTO>;

using IEnumerable_NoteDTO_t =
    System.Collections.Generic.IEnumerable<
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.NoteDTO>;

using NotesApiClient =
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Services.NotesApiClient;

using Task = System.Threading.Tasks.Task;

using Task_Nullable_NoteDTO_t =
    System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.NoteDTO?>;

namespace NullPointersEtc.NotesJournalApp.NotesBlazorFront.States;

public class NotesState
{
    public NotesState(NotesApiClient api)
    {
        api1 = api;
        notes1 = new();
        selectedNote1 = null;
    }


    public List_NoteDTO_t Notes { get => notes1; }

    public NoteDTO? SelectedNote { get => selectedNote1; }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();


    public async Task LoadAsync()
    {
        IEnumerable_NoteDTO_t list = await api1.GetAllAsync();
        notes1 = list.ToList();
        NotifyStateChanged();
    }


    public async Task SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            IEnumerable_NoteDTO_t list = await api1.GetAllAsync();
            notes1 = list.ToList();
        }
        else
        {
            IEnumerable_NoteDTO_t results = await api1.SearchAsync(query);
            notes1 = results.ToList();
        }

        NotifyStateChanged();
    }


    public async Task SelectAsync(Guid id)
    {
        selectedNote1=await api1.GetAsync(id);
        NotifyStateChanged();
    }


    public async Task_Nullable_NoteDTO_t CreateAsync(
        string title, string body)
    {
        NoteDTO? created =
            await api1.CreateAsync(title, body);

        if (created is not null)
        {
            notes1.Add(created);
            selectedNote1 = created;
            NotifyStateChanged();
        }

        return created;
    }


    public async Task_Nullable_NoteDTO_t UpdateAsync(
        Guid noteID, string title, string body)
    {
        NoteDTO? updated = await api1.UpdateAsync(
            noteID, title, body);

        if (updated is not null)
        {
            int index = notes1.FindIndex(
                n => n.Id == noteID);

            if (index >= 0)
                notes1[index] = updated;

            if (selectedNote1?.Id == noteID)
                selectedNote1 = updated;

            NotifyStateChanged();
        }

        return updated;
    }


    public async Task DeleteAsync(Guid noteID)
    {
        await api1.DeleteAsync(noteID);

        notes1.RemoveAll(n => n.Id == noteID);

        if (selectedNote1?.Id == noteID)
            selectedNote1 = null;

        NotifyStateChanged();
    }

    private readonly NotesApiClient api1;
    private List_NoteDTO_t notes1;
    private NoteDTO? selectedNote1;
}
#endregion "NotesState.cs"
