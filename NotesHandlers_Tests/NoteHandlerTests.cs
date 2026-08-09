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
using MoqReturnsExtensions = Moq.ReturnsExtensions;
using Times = Moq.Times;

using MockINoteRepository =
    Moq.Mock<NullPointersEtc.NotesJournalApp.NoteEntity.INoteRepository>;
using NullPointersEtc.NotesJournalApp.NoteEntity;

namespace NullPointersEtc.NotesJournalApp.NotesHandlers_Tests;

public class NoteHandlerTests
{
    public NoteHandlerTests()
    {
        mockRepo = new MockINoteRepository();
        noteHandler = new NoteHandler(mockRepo.Object);
    }


    [@Fact]
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

        MoqReturnsExtensions.ReturnsAsync(mockRepo.Setup(
            iNoteRepository =>
                iNoteRepository.CreateNoteAsync(It.IsAny<Note>())),
            expected);

        Note actual = await noteHandler.CreateNoteWithHandlerAsync(
            title: expected.Title, body: expected.Body);

        Assert.Equal(expected.Title, actual.Title);
        Assert.Equal(expected.Body, actual.Body);

        mockRepo.Verify(iNoteRepository =>
            iNoteRepository.CreateNoteAsync(It.IsAny<Note>()),
            Times.Once);
    }

    private readonly MockINoteRepository mockRepo;
    private readonly INoteHandler noteHandler;
}
#endregion "NotesHandlers_Tests/NoteHandlerTests.cs"
