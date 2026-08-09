#region "NotesBackEnd/UsersRestAPI.cs"
#pragma warning disable IDE0001, IDE0044, IDE0130
#pragma warning disable IDE0240, IDE0251, IDE0290
#nullable enable

using IUserHandler =
    NullPointersEtc.NotesJournalApp.NotesHandlers.IUserHandler;

using DateTime = System.DateTime;
using Guid = System.Guid;
using User = NullPointersEtc.NotesJournalApp.UserEntity.User;

using Users = System.Collections.Generic.IEnumerable<
    NullPointersEtc.NotesJournalApp.UserEntity.User>;

using ApiController = Microsoft.AspNetCore.Mvc.ApiControllerAttribute;
using ControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;
using Enumerable = System.Linq.Enumerable;
using HttpDelete = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGet = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPost = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPut = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;

using TaskReturningIActionResult = System.Threading.Tasks.Task<
    Microsoft.AspNetCore.Mvc.IActionResult>;

namespace NullPointersEtc.NotesJournalApp.NotesBackEnd;

[type: ApiController, Route("api/users")]
public sealed class UsersRestAPI : ControllerBase
{
    public UsersRestAPI(IUserHandler handler)
    {
        myHandler = handler;
    }


    [method: HttpPost]
    public async TaskReturningIActionResult HttpPostCreateUserAsync(
        CreateUserDTO user)
    {
        User user2 = await myHandler.CreateUserWithHandlerAsync(
            userName: user.UserName,
            displayName: user.DisplayName,
            eMail: user.EMailAddress);

        return Ok(new UserDTO(user2));
    }


    [HttpGet("{userID:guid:required}")]
    public async TaskReturningIActionResult HttpGetUserFromUserIdAsync(
        Guid userID)
    {
        User user1 =
            await myHandler.GetUserFromUserIdWithHandlerAsync(userID);

        return Ok(new UserDTO(user1));
    }


    [HttpGet("uname/{userName:required}")]
    public async TaskReturningIActionResult HttpGetUserFromUserNameAsync(
            string userName)
    {
        User user1 =
            await myHandler.GetUserFromUserNameWithHandlerAsync(userName);

        return Ok(new UserDTO(user1));
    }


    [HttpGet("dname/{displayName:required}")]
    public async TaskReturningIActionResult HttpGetUserFromDisplayAsyncName(
        string displayName)
    {
        User user1 = await
            myHandler.GetUserFromDisplayNameWithHandlerAsync(displayName);

        return Ok(new UserDTO(user1));
    }


    [HttpGet]
    public async TaskReturningIActionResult HttpGetAllUsersAsync()
    {
        Users users = await myHandler.GetAllUsersWithHandlerAsync();

        return Ok(Enumerable.Select(users,
            selector: user => new UserDTO(user)));
    }


    [HttpPut("{userID:guid:required}")]
    public async TaskReturningIActionResult HttpPutUpdatedUserAsync(
        Guid userID, UpdateUserDTO user)
    {
        User user1 = await myHandler.UpdateUserWithHandlerAsync(
            userID: userID,
            displayName: user.DisplayName,
            eMailAddress: user.EMail);

        return Ok(new UserDTO(user1));
    }


    [HttpDelete("{userID:guid}")]
    public async TaskReturningIActionResult HttpDeleteUserAsync(
        Guid userID)
    {
        await myHandler.DeleteWithHandlerAsync(userID);
        return NoContent();
    }


    private readonly IUserHandler myHandler;
}


public sealed class UserDTO
{
    public UserDTO(User user)
    {
        userIdField = user.UserID;
        userNameField = user.UserName;
        displayNameField = user.DisplayName;
        eMailAddressField = user.EMailAddress;
        createdAtField = user.CreatedAt;
        lastModifiedAtField = user.LastModifiedAt;
    }

    public Guid UserID { get => userIdField; }
    public string UserName { get => userNameField; }
    public string DisplayName { get => displayNameField; }
    public string EMailAddress { get => eMailAddressField; }
    public DateTime CreatedAt { get => createdAtField; }
    public DateTime UpdatedAt { get => lastModifiedAtField; }

    private readonly Guid userIdField;
    private readonly string userNameField;
    private readonly string displayNameField;
    private readonly string eMailAddressField;
    private readonly DateTime createdAtField;
    private readonly DateTime lastModifiedAtField;
}


public sealed class CreateUserDTO
{
    public CreateUserDTO(string userName,
        string displayName, string eMailAddress)
    {
        userNameField = userName;
        displayNameField = displayName;
        eMailAddressField = eMailAddress;
    }

    public string UserName { get => userNameField; }
    public string DisplayName { get => displayNameField; }
    public string EMailAddress { get => eMailAddressField; }

    private readonly string userNameField;
    private readonly string displayNameField;
    private readonly string eMailAddressField;
}


public sealed class UpdateUserDTO
{
    public UpdateUserDTO(
        string displayName, string eMailAddress)
    {
        displayNameField = displayName;
        eMailAddressField = eMailAddress;
    }

    public string DisplayName { get => displayNameField; }
    public string EMail { get => eMailAddressField; }

    private readonly string displayNameField;
    private readonly string eMailAddressField;
}
#endregion "NotesBackEnd/UsersRestAPI.cs"
