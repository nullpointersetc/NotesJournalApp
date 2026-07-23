#region namespace UserEntity
#pragma warning disable IDE0130, IDE0240
#nullable enable

using EmailAddress =
    System.ComponentModel.DataAnnotations.EmailAddressAttribute;

namespace NullPointersEtc.NotesJournalApp.UserEntity;

#region class User - the actual entity class
public class User
{
    public Guid Id
    {
        get => myGuid ?? throw new UserIdIsNotSetException();
        set => myGuid = value;
    }

    public string Identifier
    {
        get => myIdentifier ?? throw new UserIdentifierIsNotSetException();

        set => myIdentifier = string.IsNullOrWhiteSpace(value)
                ? throw new UserIdentifierIsEmptyException()
                : IdentifierIsValid(value) ? value
                : throw new UserIdentifierIsNotValidException();
    }

    public string Display
    {
        get => myName ?? throw new UserNameIsNotSetException();

        set => myName = string.IsNullOrWhiteSpace(value)
                ? throw new UserNameIsEmptyException()
                : NameIsValid(value) ? value
                : throw new UserNameIsNotValidException();
    }

    [EmailAddress]
    public string EMail
    {
        get => myEMail ?? throw new UserEMailIsNotSetException();

        set => myEMail = string.IsNullOrWhiteSpace(value)
                ? throw new UserEMailIsEmptyException()
                : EMailIsValid(value) ? value
                : throw new UserEMailIsNotValidException();
    }

    public DateTime CreatedAt
    {
        get => myCreatedAt ?? throw new UserCreatedAtIsNotSetException();
        set => myCreatedAt = value;
    }

    public DateTime UpdatedAt
    {
        get => myUpdatedAt ?? throw new UserUpdatedAtIsNotSetException();
        set => myUpdatedAt = value;
    }

    public static bool IdentifierIsValid(string identifier)
        => identifier.Length >= 1 && identifier.Length <= 32;

    public static bool NameIsValid(string name)
        => name.Length >= 1 && name.Length <= 128;

    public static bool EMailIsValid(string eMail)
        => eMail.Length >= 1 && eMail.Length <= 128
            && eMail.Contains('@');

    private System.Guid? myGuid = null;

    private string? myIdentifier = null;

    private string? myName = null;

    [EmailAddress]
    private string? myEMail = null;

    private System.DateTime? myCreatedAt = null;

    private System.DateTime? myUpdatedAt = null;
}
#endregion class Entity


#region interface IUserRepository - what Storage layer needs
public interface IUserRepository
{
    System.Threading.Tasks.Task<User>
        CreateAsync(User user);

    System.Threading.Tasks.Task<User>
        GetByIdAsync(Guid userID);

    System.Threading.Tasks.Task<User>
        GetByIdentifierAsync(string identifier);

    System.Threading.Tasks.Task<User>
        GetByDisplayAsync(string display);

    System.Threading.Tasks.Task<
        System.Collections.Generic.IEnumerable<User>>
        GetAllUsersAsync();

    System.Threading.Tasks.Task<User>
        UpdateAsync(User user);

    System.Threading.Tasks.Task DeleteAsync(Guid userID);
}
#endregion interface IUserRepository


#region Exception classes
public class UserIdIsNotSetException : System.InvalidOperationException
{
    public override string Message { get => "User Id must be set first"; }
}


public class UserIdentifierIsNotSetException : System.InvalidOperationException
{
    public override string Message { get => "User Identifier must be set first"; }
}


public class UserIdentifierIsEmptyException : System.ArgumentException
{
    public override string Message { get => "User Identifier must not be empty"; }
}


public class UserIdentifierIsNotValidException : System.ArgumentException
{
    public override string Message { get => "User Identifier must be a legal C# identifier"; }
}


public class UserNameIsNotSetException : System.InvalidOperationException
{
    public override string Message { get => "User Name must be set first"; }
}


public class UserNameIsEmptyException : System.ArgumentException
{
    public override string Message { get => "User Name must not be empty"; }
}


public class UserNameIsNotValidException : System.ArgumentException
{
    public override string Message { get => "User Name must be valid"; }
}


public class UserEMailIsNotSetException : System.InvalidOperationException
{
    public override string Message { get => "User EMail must be set first"; }
}


public class UserEMailIsEmptyException : System.ArgumentException
{
    public override string Message { get => "User EMail must not be empty"; }
}

public class UserEMailIsNotValidException : System.ArgumentException
{
    public override string Message { get => "User EMail must be of the form username@example.com"; }
}

public class UserCreatedAtIsNotSetException : System.InvalidOperationException
{
    public override string Message { get => "User CreatedAt must be set first"; }
}


public class UserUpdatedAtIsNotSetException : System.InvalidOperationException
{
    public override string Message { get => "User UpdatedAt must be set first"; }
}
#endregion Exception classes
#endregion namespace UserEntity
