#region "UsersApiClient.cs"
#pragma warning disable IDE0290, IDE0301

using UserDTO =
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.UserDTO;

using CreateUserDTO =
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.CreateUserDTO;

using UpdateUserDTO =
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.UpdateUserDTO;

using HttpClient_t = System.Net.Http.HttpClient;

using IEnumerable_UserDTO_t =
    System.Collections.Generic.IEnumerable<
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.UserDTO>;

using Task_IEnumerable_UserDTO_t =
    System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.UserDTO>>;

using Task_Nullable_UserDTO_t =
    System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models.UserDTO?>;
using System.ComponentModel.DataAnnotations;

namespace NullPointersEtc.NotesJournalApp.NotesBlazorFront.Services;

public class UsersApiClient
{
    public UsersApiClient(HttpClient_t http)
    {
        http1 = http;
    }


    public async Task_IEnumerable_UserDTO_t GetAllAsync()
    {
        return await http1.GetFromJsonAsync<
            IEnumerable_UserDTO_t>("api/users")
            ?? Enumerable.Empty<UserDTO>();
    }


    public async Task_Nullable_UserDTO_t GetbyIdAsync(System.Guid userID)
    {
        return await http1.GetFromJsonAsync<UserDTO>(
            "api/users/" + userID);
    }


    public async Task_Nullable_UserDTO_t GetByIdentifier(
        string identifier)
    {
        return await http1.GetFromJsonAsync<UserDTO>(
            "api/users/ident/" + Uri.EscapeDataString(identifier));
    }

    public async Task_Nullable_UserDTO_t GetByDisplay(
        string display)
    {
        return await http1.GetFromJsonAsync<UserDTO>(
            "api/users/name/" + Uri.EscapeDataString(display));
    }


    public async Task_Nullable_UserDTO_t CreateAsync(
        string identifier,
        string display, string eMail)
    {
        CreateUserDTO payload = new(identifier, display, eMail);

        HttpResponseMessage response =
            await http1.PostAsJsonAsync("api/users", payload);

        return await response.Content.ReadFromJsonAsync<UserDTO>();
    }


    public async Task_Nullable_UserDTO_t UpdateAsync(
        System.Guid userID,
        string display, string eMail)
    {
        UpdateUserDTO payload = new(display, eMail);

        HttpResponseMessage response =
            await http1.PutAsJsonAsync("api/users/" + userID, payload);

        return await response.Content.ReadFromJsonAsync<UserDTO>();
    }


    public async System.Threading.Tasks.Task DeleteAsync
        (System.Guid userID)
    {
        await http1.DeleteAsync("api/users/" + userID);
    }


    private readonly HttpClient_t http1;
}
#endregion "UsersApiClient.cs"
