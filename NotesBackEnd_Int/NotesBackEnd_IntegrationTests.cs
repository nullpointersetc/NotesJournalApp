#region "NotesBackEnd_Int/NotesBackEnd_IntegrationTests.cs"
#pragma warning disable IDE0130
using System.Net.Http.Json;
using Xunit;
using Task = System.Threading.Tasks.Task;
using Microsoft.Extensions.Configuration;
using System.Linq;

using WebApplicationFactory_type =
    Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<
        NullPointersEtc.NotesJournalApp.NotesBackEnd.NotesBackEnd>;

using NoteDTO =
    NullPointersEtc.NotesJournalApp.NotesBackEnd.NoteDTO;

using CreateNoteDTO =
    NullPointersEtc.NotesJournalApp.NotesBackEnd.CreateNoteDTO;

using CreateUserDTO =
    NullPointersEtc.NotesJournalApp.NotesBackEnd.CreateUserDTO;

using IEnumerable_of_NoteDTO =
    System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.NotesBackEnd.NoteDTO>;

using Dictionary_type =
    System.Collections.Generic.Dictionary<string, string?>;

namespace NullPointersEtc.NotesJournalApp.NotesBackEnd_IntegrationTests;

public class NotesBackEnd_IntegrationTests :
    IClassFixture<WebApplicationFactory_type>
{
    private readonly WebApplicationFactory_type privateFactory;

    public NotesBackEnd_IntegrationTests(
        WebApplicationFactory_type factory)
    {
        privateFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.AddInMemoryCollection(new Dictionary_type
                {
                    ["ConnectionType"] = "Sqlite",
                    ["ConnectionString"] = "Data Source=:memory:"
                });
            });
        });
    }

    #region "NOTES TESTS"

    [Fact]
    public async Task CreateNote_ThenGetNote_RoundTrip()
    {
        var client = privateFactory.CreateClient();

        CreateNoteDTO createDto = new("Test Title", "Test Body");

        var postResponse = await client.PostAsJsonAsync("/api/notes", createDto);
        postResponse.EnsureSuccessStatusCode();

        var created = await postResponse.Content.ReadFromJsonAsync<NoteDTO>();
        Assert.NotNull(created);

        var getResponse = await client.GetAsync($"/api/notes/{created.NoteID}");
        getResponse.EnsureSuccessStatusCode();

        var fetched = await getResponse.Content.ReadFromJsonAsync<NoteDTO>();
        Assert.NotNull(fetched);

        Assert.Equal(created.NoteID, fetched.NoteID);
        Assert.Equal("Test Title", fetched.Title);
        Assert.Equal("Test Body", fetched.Body);
    }

    [Fact]
    public async Task SearchNotes_ReturnsResults()
    {
        var client = privateFactory.CreateClient();

        await client.PostAsJsonAsync("/api/notes",
            new CreateNoteDTO("Alpha", "Bravo"));

        await client.PostAsJsonAsync("/api/notes",
            new CreateNoteDTO("Charlie", "Delta"));

        var response = await client.GetAsync("/api/notes/search?query=Alpha");
        response.EnsureSuccessStatusCode();

        IEnumerable_of_NoteDTO? results =
            await response.Content.ReadFromJsonAsync<IEnumerable_of_NoteDTO>();

        Assert.NotNull(results);
        Assert.Single(results);
        Assert.Equal("Alpha", results.First().Title);
    }

    #endregion "NOTES TESTS"

    #region "USERS TESTS"

    [Fact]
    public async Task CreateUser_ThenGetUser_RoundTrip()
    {
        var client = privateFactory.CreateClient();

        var createDto = new CreateUserDTO(
            userName: "darren",
            displayName: "Darren",
            eMailAddress: "darren@example.com");

        var postResponse = await client.PostAsJsonAsync("/api/users", createDto);
        postResponse.EnsureSuccessStatusCode();

        var created = await postResponse.Content.ReadFromJsonAsync<
            NullPointersEtc.NotesJournalApp.NotesBackEnd.UserDTO>();

        Assert.NotNull(created);

        var getResponse = await client.GetAsync($"/api/users/{created.UserID}");
        getResponse.EnsureSuccessStatusCode();

        var fetched = await getResponse.Content.ReadFromJsonAsync<
        NullPointersEtc.NotesJournalApp.NotesBackEnd.UserDTO>();

        Assert.NotNull(fetched);

        Assert.Equal(created.UserID, fetched.UserID);
        Assert.Equal("darren", fetched.UserName);
        Assert.Equal("Darren", fetched.DisplayName);
        Assert.Equal("darren@example.com", fetched.EMailAddress);
    }
    #endregion "USERS TESTS"
}

#endregion "NotesBackEnd_Int/NotesBackEnd_IntegrationTests.cs"
