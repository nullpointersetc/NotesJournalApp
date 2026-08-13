#region "NotesBackEnd/UsersRestAPI.cs"
#pragma warning disable IDE0001, IDE0044, IDE0130
#pragma warning disable IDE0240, IDE0251, IDE0290
#nullable enable

using IUserHandler =
    NullPointersEtc.NotesJournalApp.NotesHandlers.IUserHandler;

using DateTime = System.DateTime;
using Guid = System.Guid;
using User = NullPointersEtc.NotesJournalApp.UserEntity.User;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Results = Microsoft.AspNetCore.Http.Results;

using Users = System.Collections.Generic.IEnumerable<
    NullPointersEtc.NotesJournalApp.UserEntity.User>;

using TaskReturningIResult = System.Threading.Tasks.Task<
        Microsoft.AspNetCore.Http.IResult>;

namespace NullPointersEtc.NotesJournalApp.NotesBackEnd;

public static class UsersRestAPI
{
    public static void MapEndpoints(
        Microsoft.AspNetCore.Builder.WebApplication app)
    {
        app.MapPost(GetOrCreateUserURI, HttpPostCreateUserAsync)
            .WithTags("Users")
            .WithSummary("Create a new user")
            .WithDescription("Creates a new user with username, display name, and email address.")
            .Accepts<CreateUserDTO>("application/json")
            .Produces<UserDTO>(StatusCodes.Status200OK);

        app.MapGet(GetOrCreateUserURI, HttpGetAllUsersAsync)
            .WithTags("Users")
            .WithSummary("Get all users")
            .WithDescription("Returns all users in the system.")
            .Produces<System.Collections.Generic.IEnumerable<UserDTO>>(StatusCodes.Status200OK);

        app.MapGet(GetOrUpdateUserURI, HttpGetUserByUserIdAsync)
            .WithTags("Users")
            .WithSummary("Get a user by ID")
            .WithDescription("Retrieves a user using their GUID identifier.")
            .Produces<UserDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPut(GetOrUpdateUserURI, HttpPutUpdateUserByUserIdAsync)
            .WithTags("Users")
            .WithSummary("Update a user")
            .WithDescription("Updates the display name and email address of an existing user.")
            .Accepts<UpdateUserDTO>("application/json")
            .Produces<UserDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapDelete(GetOrUpdateUserURI, HttpDeleteUserByUserIdAsync)
            .WithTags("Users")
            .WithSummary("Delete a user")
            .WithDescription("Deletes a user using its GUID identifier.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        app.MapGet(GetUserByNameURI, HttpGetUserByUserNameAsync)
            .WithTags("Users")
            .WithSummary("Get a user by User Name")
            .WithDescription("Retrieves a user using its user name.")
            .Produces<UserDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapGet(GetUserByDisplayName, HttpGetUserByDisplayNameAsync)
            .WithTags("Users")
            .WithSummary("Get a user by Display Name")
            .WithDescription("Retrieves a user using its display name.")
            .Produces<UserDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static string GetOrCreateUserURI
    {
        get => "/api/users";
    }

    public static async TaskReturningIResult
        HttpPostCreateUserAsync(
            IUserHandler handler, CreateUserDTO dto)
    {
        User user = await handler.CreateUserWithHandlerAsync(
            dto.UserName, dto.DisplayName, dto.EMailAddress);

        return Results.Ok(new UserDTO(user));
    }

    public static async TaskReturningIResult
        HttpGetAllUsersAsync(
            IUserHandler handler)
    {
        Users users = await handler.GetAllUsersWithHandlerAsync();

        return Results.Ok(users.Select<User, UserDTO>(
            user => new UserDTO(user)));
    }

    private static string GetOrUpdateUserURI
    {
        get => "/api/users/{userID:guid:required}";
    }

    public static async TaskReturningIResult
        HttpGetUserByUserIdAsync(
            IUserHandler handler, Guid userID)
    {
        User user =
            await handler.GetUserFromUserIdWithHandlerAsync(userID);

        return Results.Ok(new UserDTO(user));
    }

    public static async TaskReturningIResult
        HttpPutUpdateUserByUserIdAsync(
            IUserHandler handler, Guid userID, UpdateUserDTO dto)
    {
        var updated = await handler.UpdateUserWithHandlerAsync(
            userID, dto.DisplayName, dto.EMailAddress);

        return Results.Ok(new UserDTO(updated));
    }

    public static async TaskReturningIResult
        HttpDeleteUserByUserIdAsync(
            IUserHandler handler, Guid userID)
    {
        await handler.DeleteWithHandlerAsync(userID);
        return Results.NoContent();
    }


    private static string GetUserByNameURI
    {
        get => "/api/usern/{userName:required}";

    }

    public static async TaskReturningIResult
        HttpGetUserByUserNameAsync(
            IUserHandler handler, string userName)
    {
        User user1 =
            await handler.GetUserFromUserNameWithHandlerAsync(userName);

        return Results.Ok(new UserDTO(user1));
    }

    private static string GetUserByDisplayName
    {
        get => "/api/userd/{displayName:required}";
    }

    public static async TaskReturningIResult
        HttpGetUserByDisplayNameAsync(
            IUserHandler handler, string displayName)
    {
        User user1 = await
            handler.GetUserFromDisplayNameWithHandlerAsync(displayName);

        return Results.Ok(new UserDTO(user1));
    }
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
    public string EMailAddress { get => eMailAddressField; }

    private readonly string displayNameField;
    private readonly string eMailAddressField;
}
#endregion "NotesBackEnd/UsersRestAPI.cs"
