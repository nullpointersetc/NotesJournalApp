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
    public async Task CreateUserCallsRepoAndReturnsUserAsync()
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
            iUserRepo => iUserRepo.CreateUserAsync(
                It.IsAny<User>())), expected);

        User actual = await iUserHandler.CreateUserWithHandlerAsync(
            userName: expected.UserName,
            displayName: expected.DisplayName,
            eMail: expected.EMailAddress);

        Assert.Equal(expected.UserName, actual.UserName);
        Assert.Equal(expected.DisplayName, actual.DisplayName);
        Assert.Equal(expected.EMailAddress, actual.EMailAddress);

        mockRepo.Verify(
            iUserRepo => iUserRepo.CreateUserAsync(It.IsAny<User>()),
            Times.Once);
    }


    [method: @Fact]
    public async Task GetUserByIdCallsRepoAsync()
    {
        Guid id = Guid.NewGuid();
        await iUserHandler.GetUserFromUserIdWithHandlerAsync(id);

        mockRepo.Verify(iUserRepo => iUserRepo.GetUserByUserIdAsync(id),
            Times.Once);
    }


    [method: @Fact]
    public async Task GetUserByUserNameCallsRepoAsync()
    {
        await iUserHandler.GetUserFromUserNameWithHandlerAsync("abc");

        mockRepo.Verify(iUserRepo => iUserRepo.GetUserByUserNameAsync("abc"),
            Times.Once);
    }


    [method: @Fact]
    public async Task GetUserByDisplayNameCallsRepoAsync()
    {
        await iUserHandler.GetUserFromDisplayNameWithHandlerAsync("display");

        mockRepo.Verify(
            iUserRepo => iUserRepo.GetUserByDisplayNameAsync("display"),
            Times.Once);
    }


    [method: @Fact]
    public async Task GetAllUsersCallsRepoAsync()
    {
        await iUserHandler.GetAllUsersWithHandlerAsync();
        mockRepo.Verify(iUserRepo => iUserRepo.GetAllUsersAsync(), Times.Once);
    }


    [method: @Fact]
    public async Task UpdateUserMutatesFieldsAndCallsRepoAsync()
    {
        Guid id = Guid.NewGuid();

        User existing = new()
        {
            UserID = id,
            UserName = "u",
            DisplayName = "old",
            EMailAddress = "old@mail.com",
            CreatedAt = DateTime.UtcNow.AddHours(-1),
            LastModifiedAt = DateTime.UtcNow.AddHours(-1)
        };

        ReturnsExtensions.ReturnsAsync(
            mockRepo.Setup(iUserRepo => iUserRepo.GetUserByUserIdAsync(id)),
            existing);

        ReturnsExtensions.ReturnsAsync(
            mockRepo.Setup(iUserRepo => iUserRepo.UpdateUserAsync(existing)),
            existing);

        User result = await iUserHandler.UpdateUserWithHandlerAsync(
            id, "new", "new@mail.com");

        Assert.Equal("new", existing.DisplayName);
        Assert.Equal("new@mail.com", existing.EMailAddress);
        Assert.True(existing.LastModifiedAt > existing.CreatedAt);

        mockRepo.Verify(iUserRepo => iUserRepo.UpdateUserAsync(existing),
            Times.Once);
    }


    [method: @Fact]
    public async Task DeleteUserCallsRepository()
    {
        Guid id = Guid.NewGuid();
        await iUserHandler.DeleteWithHandlerAsync(id);
        mockRepo.Verify(iUserRepo => iUserRepo.DeleteUserAsync(id), Times.Once);
    }

    private readonly MockIUserRepository mockRepo;
    private readonly IUserHandler iUserHandler;
}
#endregion "NotesHandlers_Tests/UserHandlerTests.cs"
