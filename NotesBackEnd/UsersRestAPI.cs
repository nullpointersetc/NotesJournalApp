#region UsersRestAPI classes
#pragma warning disable IDE0001, IDE0044, IDE0130
#pragma warning disable IDE0251, IDE0290
#nullable enable

using Microsoft.AspNetCore.Mvc;

using IUserHandler =
    NullPointersEtc.NotesJournalApp.UserHandler.IUserHandler;

using User = NullPointersEtc.NotesJournalApp.UserEntity.User;

namespace NullPointersEtc.NotesJournalApp.UserRestAPI;

[type: ApiController, Route("api/users")]
public class UsersREST_API : ControllerBase
{
    public UsersREST_API(IUserHandler handler)
    {
        handler1 = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateUserDTO user2)
    {
        User user1 = await handler1.CreateAsync(
            identifier: user2.Identifier,
            display: user2.Display, eMail: user2.EMail);

        return Ok(new UserDTO(user1));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        User user1 = await handler1.GetByGuidAsync(id);
        return Ok(new UserDTO(user1));
    }

    [HttpGet("ident/{identifier}")]
    public async Task<IActionResult> GetByIdentifier(
        string identifier)
    {
        User user1 = await handler1.GetByIdentifierAsync(identifier);
        return Ok(new UserDTO(user1));
    }

    [HttpGet("name/{display}")]
    public async Task<IActionResult> GetByDisplay(string display)
    {
        User user1 = await handler1.GetByDisplayAsync(display);
        return Ok(new UserDTO(user1));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        System.Collections.Generic.IEnumerable<User>
            users = await handler1.GetAllAsync();
        return Ok(users.Select(user => new UserDTO(user)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id, UpdateUserDTO user)
    {
        User user1 = await handler1.UpdateAsync(
            userID: id,
            display: user.Display, eMail: user.EMail);

        return Ok(new UserDTO(user1));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await handler1.DeleteAsync(id);
        return NoContent();
    }

    private readonly IUserHandler handler1;
}


public class UserDTO
{
    public UserDTO(User user)
    {
        id1 = user.Id;
        ident1 = user.Identifier;
        display1 = user.Display;
        eMail1 = user.EMail;
        created1 = user.CreatedAt;
        modified1 = user.UpdatedAt;
    }

    public Guid Id { get => id1; }
    public string Identifier { get => ident1; }
    public string Display { get => display1; }
    public string EMail { get => eMail1; }
    public DateTime CreatedAt { get => created1; }
    public DateTime UpdatedAt { get => modified1; }

    private readonly Guid id1;
    private readonly string ident1;
    private readonly string display1;
    private readonly string eMail1;
    private readonly DateTime created1;
    private readonly DateTime modified1;
}


public class CreateUserDTO
{
    public CreateUserDTO(string identifier,
        string display, string eMail)
    {
        identifier1 = identifier;
        displayName1 = display;
        eMailAddress1 = eMail;
    }

    public string Identifier { get => identifier1; }
    public string Display { get => displayName1; }
    public string EMail { get => eMailAddress1; }

    private string identifier1;
    private readonly string displayName1;
    private readonly string eMailAddress1;
}


public class UpdateUserDTO
{
    public UpdateUserDTO(string display, string eMail)
    {
        displayName1 = display;
        eMailAddress1 = eMail;
    }

    public string Display { get => displayName1; }
    public string EMail { get => eMailAddress1; }

    private readonly string displayName1;
    private readonly string eMailAddress1;
}
#endregion UsersRestAPI classes
