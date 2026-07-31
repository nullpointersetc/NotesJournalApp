#region "UsersState.cs"
#pragma warning disable IDE0028, IDE0290, IDE0305

using UserDTO =
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.UserDTO;

using List_UserDTO_t =
    System.Collections.Generic.List<
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.UserDTO>;

using IEnumerable_UserDTO_t =
    System.Collections.Generic.IEnumerable<
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.UserDTO>;

using UsersApiClient =
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Services.UsersApiClient;

using Task = System.Threading.Tasks.Task;

using Task_Nullable_UserDTO_t =
    System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.UserDTO?>;

namespace NullPointersEtc.NotesJournalApp.NotesBlazorFront.States;

public class UsersState
{
    public UsersState(UsersApiClient api)
    {
        api1 = api;
        notes1 = new();
        selectedNote1 = null;
    }


    public List_UserDTO_t Users { get => notes1; }

    public UserDTO? SelectedUser { get => selectedNote1; }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();


    public async Task LoadAsync()
    {
        IEnumerable_UserDTO_t list = await api1.GetAllAsync();
        notes1 = list.ToList();
        NotifyStateChanged();
    }


    public async Task SelectAsync(Guid id)
    {
        selectedNote1 = await api1.GetbyIdAsync(id);
        NotifyStateChanged();
    }


    public async Task_Nullable_UserDTO_t CreateAsync(
        string identifier, string display, string eMail)
    {
        UserDTO? created =
            await api1.CreateAsync(identifier: identifier,
                display: display, eMail: eMail);

        if (created is not null)
        {
            notes1.Add(created);
            selectedNote1 = created;
            NotifyStateChanged();
        }

        return created;
    }


    public async Task_Nullable_UserDTO_t UpdateAsync(
        Guid userID, string display, string eMail)
    {
        UserDTO? updated = await api1.UpdateAsync(
            userID, display: display, eMail: eMail);

        if (updated is not null)
        {
            int index = notes1.FindIndex(
                n => n.UserID == userID);

            if (index >= 0)
                notes1[index] = updated;

            if (selectedNote1?.UserID == userID)
                selectedNote1 = updated;

            NotifyStateChanged();
        }

        return updated;
    }


    public async Task DeleteAsync(Guid userID)
    {
        await api1.DeleteAsync(userID);

        notes1.RemoveAll(n => n.UserID == userID);

        if (selectedNote1?.UserID == userID)
            selectedNote1 = null;

        NotifyStateChanged();
    }

    private readonly UsersApiClient api1;
    private List_UserDTO_t notes1;
    private UserDTO? selectedNote1;
}
#endregion "UsersState.cs"
