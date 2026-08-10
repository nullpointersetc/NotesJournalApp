#region "NotesHandlers_Tests/NoteHandlerTests.cs"
#pragma warning disable IDE0130, CA1859

using INoteHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.INoteHandler;
using NoteHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.NoteHandler;
using DateTime = System.DateTime;
using Guid = System.Guid;
using Assert = Xunit.Assert;
using Fact = Xunit.FactAttribute;
using Note = NullPointersEtc.NotesJournalApp.NoteEntity.Note;
using Task = System.Threading.Tasks.Task;
using It = Moq.It;
using ReturnsExtensions = Moq.ReturnsExtensions;
using Times = Moq.Times;

using MockINoteRepository =
    Moq.Mock<NullPointersEtc.NotesJournalApp.NoteEntity.INoteRepository>;

namespace NullPointersEtc.NotesJournalApp.NotesHandlers_Tests;

public class NoteHandlerTests
{
    public NoteHandlerTests()
    {
        mockRepo = new MockINoteRepository();
        noteHandler = new NoteHandler(mockRepo.Object);
    }


    [method: @Fact]
    public async Task CreateNoteCallsRepoAndReturnsNoteAsync()
    {
        Note expected = new()
        {
            NoteID = Guid.NewGuid(),
            Title = "This is a sample title",
            Body = "This is a sample body",
            CreatedAt = DateTime.UtcNow,
            LastModifiedAt = DateTime.UtcNow
        };

        ReturnsExtensions.ReturnsAsync(mockRepo.Setup(
            iNoteRepo => iNoteRepo.CreateNoteAsync(It.IsAny<Note>())),
            expected);

        Note actual = await noteHandler.CreateNoteWithHandlerAsync(
            title: expected.Title, body: expected.Body);

        Assert.Equal(expected.Title, actual.Title);
        Assert.Equal(expected.Body, actual.Body);

        mockRepo.Verify(iNoteRepo =>
            iNoteRepo.CreateNoteAsync(It.IsAny<Note>()),
            Times.Once);
    }


    [method: @Fact]
    public async Task GetAllNotesCallsRepository()
    {
        await noteHandler.GetAllNotesWithHandlerAsync();

        mockRepo.Verify(
            iNoteRepo => iNoteRepo.GetAllNotesAsync(),
            Times.Once);
    }


    [method: @Fact]
    public async Task GetNoteByIdCallsRepository()
    {
        Guid id = Guid.NewGuid();
        await noteHandler.GetNoteFromNoteIdWithHandlerAsync(id);

        mockRepo.Verify(
            iNoteRepo => iNoteRepo.GetNoteByIdAsync(id),
            Times.Once);
    }


    [method: @Fact]
    public async Task SearchNotesCallsRepository()
    {
        await noteHandler.SearchNotesWithHandlerAsync("abc");

        mockRepo.Verify(
            iNoteRepo => iNoteRepo.SearchNotesAsync("abc"),
            Times.Once);
    }

    [method: @Fact]
    public async Task UpdateNoteMutatesFieldsAndCallsRepository()
    {
        Guid id = Guid.NewGuid();

        var existing = new Note()
        {
            NoteID = id,
            Title = "Old",
            Body = "OldBody",
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            LastModifiedAt = DateTime.UtcNow.AddHours(-1)
        };

        ReturnsExtensions.ReturnsAsync(
            mockRepo.Setup(iNoteRepo => iNoteRepo.GetNoteByIdAsync(id)),
                existing);

        ReturnsExtensions.ReturnsAsync(
            mockRepo.Setup(iNoteRepo => iNoteRepo.UpdateNoteAsync(existing)),
            existing);

        var result = await noteHandler.UpdateNoteWithHandlerAsync(
            id, title: "New", body: "NewBody");

        Assert.Equal("New", existing.Title);
        Assert.Equal("NewBody", existing.Body);
        Assert.True(existing.LastModifiedAt > existing.CreatedAt);

        mockRepo.Verify(iNoteRepo => iNoteRepo.UpdateNoteAsync(existing),
            Times.Once);
    }


    [method: @Fact]
    public async Task DeleteNoteCallsRepository()
    {
        Guid id = Guid.NewGuid();
        await noteHandler.DeleteNoteWithHandlerAsync(id);

        mockRepo.Verify(iNoteRepo => iNoteRepo.DeleteNoteAsync(id),
            Times.Once);
    }


    private readonly MockINoteRepository mockRepo;
    private readonly INoteHandler noteHandler;
}
#endregion "NotesHandlers_Tests/NoteHandlerTests.cs"
