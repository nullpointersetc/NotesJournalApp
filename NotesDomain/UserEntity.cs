#region "NotesDomain/UserEntity.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240
#nullable enable

using DateTime = System.DateTime;
using System.Linq;
using Guid = System.Guid;
using Key = System.ComponentModel.DataAnnotations.KeyAttribute;
using StringLength = System.ComponentModel.DataAnnotations.StringLengthAttribute;
using EmailAddress = System.ComponentModel.DataAnnotations.EmailAddressAttribute;
using InvalidOperationException = System.InvalidOperationException;
using Task = System.Threading.Tasks.Task;

using TaskReturningUser = System.Threading.Tasks.Task<
    NullPointersEtc.NotesJournalApp.UserEntity.User>;

using TaskReturningUsers = System.Threading.Tasks.Task<
    System.Collections.Generic.IEnumerable<
        NullPointersEtc.NotesJournalApp.UserEntity.User>>;

namespace NullPointersEtc.NotesJournalApp.UserEntity;

public sealed class User
{
    [property: Key]
    public Guid UserID
    {
        get => userIdField ?? throw new UserIdIsNotSetException();
        set => userIdField = value;
    }

    [property: StringLength(
        maximumLength: MAX_USER_NAME_LENGTH,
        MinimumLength = 1)]
    public string UserName
    {
        get => userNameField ?? throw new UserNameIsNotSetException();

        set => userNameField = string.IsNullOrWhiteSpace(value)
            ? throw new UserNameIsEmptyException()
            : value.Length > MAX_USER_NAME_LENGTH
            ? throw new UserNameIsTooLongException()
            : IdentifierIsValid(value)
            ? value
            : throw new UserNameIsNotValidException();
    }

    [property: StringLength(
        maximumLength: MAX_DISPLAY_NAME_LENGTH,
        MinimumLength = 1)]
    public string DisplayName
    {
        get => displayNameField ?? throw new UserNameIsNotSetException();

        set => displayNameField = string.IsNullOrWhiteSpace(value)
            ? throw new DisplayNameIsEmptyException()
            : value.Length > MAX_DISPLAY_NAME_LENGTH
            ? throw new DisplayNameIsTooLongException()
            : value;
    }

    [property: EmailAddress,
    StringLength(
        maximumLength: MAX_EMAIL_ADDRESS_LENGTH,
        MinimumLength = 1)]
    public string EMailAddress
    {
        get => eMailAddressField ?? throw new EMailAddressIsNotSetException();

        set => eMailAddressField = string.IsNullOrWhiteSpace(value)
                ? throw new EMailAddressIsEmptyException()
                : value.Length > MAX_EMAIL_ADDRESS_LENGTH
                ? throw new EmailAddressIsTooLongException()
                : EMailIsValid(value)
                ? value
                : throw new EMailAddressIsNotValidException();
    }

    public DateTime CreatedAt
    {
        get => createdAtField
                    ?? throw new UserCreatedAtIsNotSetException();

        set => createdAtField = createdAtField is null ? value
                    : createdAtField == value ? value
                    : throw new UserCreatedAtWouldBeChangedException();
    }

    public DateTime LastModifiedAt
    {
        get => lastModifiedAtField
                    ?? throw new UserLastModifiedAtIsNotSetException();

        set => lastModifiedAtField = lastModifiedAtField is null ? value
                    : lastModifiedAtField <= value ? value
                    : throw new
                        UserLastModifiedDateWouldGoBackInTimeException();
    }

    public static bool IdentifierIsValid(string identifier)
        => identifier.Length >= 1
            && identifier.Length <= MAX_USER_NAME_LENGTH
            && char.IsAsciiLetter(identifier[0])
            && identifier.All<char>(
                ch => char.IsAsciiLetterOrDigit(ch) || ch == '_');

    public static bool NameIsValid(string name)
        => name.Length >= 1
            && name.Length <= MAX_DISPLAY_NAME_LENGTH;

    public static bool EMailIsValid(string eMail)
        => eMail.Length >= 1
            && eMail.Length <= MAX_EMAIL_ADDRESS_LENGTH
            && eMail.Contains('@');

    private Guid? userIdField = null;

    private string? userNameField = null;

    private string? displayNameField = null;

    [EmailAddress]
    private string? eMailAddressField = null;

    private DateTime? createdAtField = null;

    private DateTime? lastModifiedAtField = null;

    public const int MAX_USER_NAME_LENGTH = 32;
    public const int MAX_DISPLAY_NAME_LENGTH = 128;
    public const int MAX_EMAIL_ADDRESS_LENGTH = 128;
}


public interface IUserRepository
{
    TaskReturningUser CreateUserAsync(User user);

    TaskReturningUser GetUserByUserIdAsync(Guid userID);

    TaskReturningUser GetUserByUserNameAsync(string userName);

    TaskReturningUser GetUserByDisplayNameAsync(string displayName);

    TaskReturningUsers GetAllUsersAsync();

    TaskReturningUser UpdateUserAsync(User user);

    Task DeleteUserAsync(Guid userID);
}


public sealed class UserIdIsNotSetException : InvalidOperationException
{
    public override string Message { get => "User Id must be set first"; }
}


public sealed class UserNameIsNotSetException : InvalidOperationException
{
    public override string Message { get => "UserName must be set first"; }
}


public sealed class UserNameIsEmptyException : System.ArgumentException
{
    public override string Message { get => "UserName must not be empty"; }
}


public sealed class UserNameIsTooLongException : System.ArgumentException
{
    public override string Message
    {
        get => "UserName must be " +
            User.MAX_USER_NAME_LENGTH + " characters or shorter";
    }
}


public sealed class UserNameIsNotValidException : System.ArgumentException
{
    public override string Message { get => "UserName must be a legal C# identifier"; }
}


public sealed class DisplayNameIsNotSetException : InvalidOperationException
{
    public override string Message { get => "DisplayName must be set first"; }
}


public sealed class DisplayNameIsEmptyException : System.ArgumentException
{
    public override string Message { get => "DisplayName must not be empty"; }
}


public sealed class DisplayNameIsTooLongException : System.ArgumentException
{
    public override string Message
    {
        get => "DisplayName must be " +
            User.MAX_DISPLAY_NAME_LENGTH + " characters or shorter";
    }
}


public sealed class EMailAddressIsNotSetException : InvalidOperationException
{
    public override string Message { get => "User EMail must be set first"; }
}


public sealed class EMailAddressIsEmptyException : System.ArgumentException
{
    public override string Message { get => "User EMail must not be empty"; }
}


public sealed class EMailAddressIsNotValidException : System.ArgumentException
{
    public override string Message { get => "User EMail must be of the form username@example.com"; }
}


public sealed class EmailAddressIsTooLongException : System.ArgumentException
{
    public override string Message
    {
        get => "EmailAddress must be " +
            User.MAX_EMAIL_ADDRESS_LENGTH + " characters or fewer";
    }
}


public sealed class UserCreatedAtIsNotSetException : InvalidOperationException
{
    public override string Message { get => "User CreatedAt must be set first"; }
}


public sealed class UserCreatedAtWouldBeChangedException
    : InvalidOperationException
{
    public override string Message
    {
        get => "User CreatedAt cannot be changed. " +
            "This is a data-integrity issue";
    }
}


public sealed class UserLastModifiedAtIsNotSetException : InvalidOperationException
{
    public override string Message { get => "User LastModifiedAt must be set first"; }
}


public sealed class UserLastModifiedDateWouldGoBackInTimeException
    : InvalidOperationException
{
    public override string Message
    {
        get => "User LastModifiedAt cannot be set to an earlier time. " +
            "This is a data-integrity issue";
    }
}
#endregion "NotesDomain/UserEntity.cs"
