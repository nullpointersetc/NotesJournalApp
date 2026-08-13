#region "NotesBackEnd_Int/NotesBackEnd_IntegrationTests.cs"
#pragma warning disable IDE0130
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace NullPointersEtc.NotesJournalApp.NotesBackEnd_IntegrationTests;

public class NotesBackEnd_IntegrationTests :
    IClassFixture<WebApplicationFactory<
    NullPointersEtc.NotesJournalApp.NotesBackEnd.NotesBackEnd>>
{
    private readonly WebApplicationFactory<
        NullPointersEtc.NotesJournalApp.NotesBackEnd.NotesBackEnd> factory;

    public NotesBackEnd_IntegrationTests(
        WebApplicationFactory<
            NullPointersEtc.NotesJournalApp.NotesBackEnd.NotesBackEnd> factory)
    {
        this.factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Database:default:Type"] = "Sqlite",
                    ["Database:default:ConnectionString"] = "Data Source=:memory:"
                });
            });
        });
    }

    #region "NOTES TESTS"

    [Fact]
    public async Task CreateNote_ThenGetNote_RoundTrip()
    {
        var client = factory.CreateClient();

        var createDto = new NullPointersEtc.NotesJournalApp.NotesBackEnd.CreateNoteDTO("Test Title", "Test Body");

        var postResponse = await client.PostAsJsonAsync("/api/notes", createDto);
        postResponse.EnsureSuccessStatusCode();

        var created = await postResponse.Content.ReadFromJsonAsync<NullPointersEtc.NotesJournalApp.NotesBackEnd.NoteDTO>();
        Assert.NotNull(created);

        var getResponse = await client.GetAsync($"/api/notes/{created.NoteID}");
        getResponse.EnsureSuccessStatusCode();

        var fetched = await getResponse.Content.ReadFromJsonAsync<NullPointersEtc.NotesJournalApp.NotesBackEnd.NoteDTO>();
        Assert.NotNull(fetched);

        Assert.Equal(created.NoteID, fetched.NoteID);
        Assert.Equal("Test Title", fetched.Title);
        Assert.Equal("Test Body", fetched.Body);
    }

    [Fact]
    public async Task SearchNotes_ReturnsResults()
    {
        var client = factory.CreateClient();

        await client.PostAsJsonAsync("/api/notes",
            new NullPointersEtc.NotesJournalApp.NotesBackEnd.CreateNoteDTO("Alpha", "Bravo"));

        await client.PostAsJsonAsync("/api/notes",
            new NullPointersEtc.NotesJournalApp.NotesBackEnd.CreateNoteDTO("Charlie", "Delta"));

        var response = await client.GetAsync("/api/notes/search?query=Alpha");
        response.EnsureSuccessStatusCode();

        var results = await response.Content.ReadFromJsonAsync<IEnumerable<
        NullPointersEtc.NotesJournalApp.NotesBackEnd.NoteDTO>>();
        Assert.NotNull(results);
        Assert.Single(results);
        Assert.Equal("Alpha", results.First().Title);
    }

    #endregion "NOTES TESTS"

    #region "USERS TESTS"

    [Fact]
    public async Task CreateUser_ThenGetUser_RoundTrip()
    {
        var client = factory.CreateClient();

        var createDto = new NullPointersEtc.NotesJournalApp.NotesBackEnd.CreateUserDTO(
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
