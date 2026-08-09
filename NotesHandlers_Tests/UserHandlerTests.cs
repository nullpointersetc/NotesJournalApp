#region "NotesHandlers_Tests/UserHandlerTests.cs"
#pragma warning disable IDE0130, CA1859

using IUserHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.IUserHandler;
using UserHandler = NullPointersEtc.NotesJournalApp.NotesHandlers.UserHandler;
using DateTime = System.DateTime;
using Guid = System.Guid;
using Assert = Xunit.Assert;
using Fact = Xunit.FactAttribute;
using User = NullPointersEtc.NotesJournalApp.UserEntity.User;
using Task = System.Threading.Tasks.Task;
using It = Moq.It;
using ReturnsExtensions = Moq.ReturnsExtensions;
using Times = Moq.Times;

using MockIUserRepository =
    Moq.Mock<NullPointersEtc.NotesJournalApp.UserEntity.IUserRepository>;

namespace NullPointersEtc.NotesJournalApp.NotesHandlers_Tests;

public class UserHandlerTests
{
    public UserHandlerTests()
    {
        mockRepo = new MockIUserRepository();
        iUserHandler = new UserHandler(mockRepo.Object);
    }


    [method: @Fact]
    public async Task CreateNoteCallsRepoAndReturnsNoteAsync()
    {
        User expected = new()
        {
            UserID = Guid.NewGuid(),
            UserName = "sherlockholmes",
            DisplayName = "Sherlock Holmes",
            EMailAddress = "sholmes@example.net",
            CreatedAt = DateTime.UtcNow,
            LastModifiedAt = DateTime.UtcNow
        };

        ReturnsExtensions.ReturnsAsync(mockRepo.Setup(
            iUserRepository => iUserRepository.CreateUserAsync(
                It.IsAny<User>())), expected);

        User actual = await iUserHandler.CreateUserWithHandlerAsync(
            userName: expected.UserName,
            displayName: expected.DisplayName,
            eMail: expected.EMailAddress);

        Assert.Equal(expected.UserName, actual.UserName);
        Assert.Equal(expected.DisplayName, actual.DisplayName);
        Assert.Equal(expected.EMailAddress, actual.EMailAddress);

        mockRepo.Verify(iUserRepository =>
            iUserRepository.CreateUserAsync(It.IsAny<User>()),
            Times.Once);
    }

    private readonly MockIUserRepository mockRepo;
    private readonly IUserHandler iUserHandler;
}
#endregion "NotesHandlers_Tests/UserHandlerTests.cs"
