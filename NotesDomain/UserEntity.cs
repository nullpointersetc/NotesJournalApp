#region "NotesDomain/UserEntity.cs"
#pragma warning disable IDE0001, IDE0130, IDE0240
#nullable enable

using Key = System.ComponentModel.DataAnnotations.KeyAttribute;
using StringLength = System.ComponentModel.DataAnnotations.StringLengthAttribute;
using EmailAddress = System.ComponentModel.DataAnnotations.EmailAddressAttribute;

namespace NullPointersEtc.NotesJournalApp.UserEntity
{
    #region "Entity class User"
    public class User
    {
        [Key]
        public Guid UserID
        {
            get => userId1 ?? throw new UserIdIsNotSetException();
            set => userId1 = value;
        }

        [StringLength(maximumLength: MAX_USER_NAME_LENGTH,
            MinimumLength = 1)]
        public string UserName
        {
            get => userName1 ?? throw new UserNameIsNotSetException();

            set => userName1 = string.IsNullOrWhiteSpace(value)
                ? throw new UserNameIsEmptyException()
                : value.Length > MAX_USER_NAME_LENGTH
                ? throw new UserNameIsTooLongException()
                : IdentifierIsValid(value)
                ? value
                : throw new UserNameIsNotValidException();
        }

        [StringLength(maximumLength: MAX_DISPLAY_NAME_LENGTH,
            MinimumLength = 1)]
        public string Display
        {
            get => displayName1 ?? throw new UserNameIsNotSetException();

            set => displayName1 = string.IsNullOrWhiteSpace(value)
                ? throw new DisplayNameIsEmptyException()
                : value.Length > MAX_DISPLAY_NAME_LENGTH
                ? throw new DisplayNameIsTooLongException()
                : value;
        }

        [EmailAddress,
        StringLength(maximumLength: MAX_EMAIL_ADDRESS_LENGTH,
            MinimumLength = 1)]
        public string EMail
        {
            get => eMail1 ?? throw new EMailAddressIsNotSetException();

            set => eMail1 = string.IsNullOrWhiteSpace(value)
                    ? throw new EMailAddressIsEmptyException()
                    : value.Length > MAX_EMAIL_ADDRESS_LENGTH
                    ? throw new EmailAddressIsTooLongException()
                    : EMailIsValid(value)
                    ? value
                    : throw new EMailAddressIsNotValidException();
        }

        public DateTime CreatedAt
        {
            get => createdDate1 ?? throw new UserCreatedAtIsNotSetException();
            set => createdDate1 = value;
        }

        public DateTime UpdatedAt
        {
            get => lastUpdateDate1 ?? throw new UserUpdatedAtIsNotSetException();
            set => lastUpdateDate1 = value;
        }

        public static bool IdentifierIsValid(string identifier)
            => identifier.Length >= 1
                && identifier.Length <= MAX_USER_NAME_LENGTH;

        public static bool NameIsValid(string name)
            => name.Length >= 1
                && name.Length <= MAX_DISPLAY_NAME_LENGTH;

        public static bool EMailIsValid(string eMail)
            => eMail.Length >= 1
                && eMail.Length <= MAX_EMAIL_ADDRESS_LENGTH
                && eMail.Contains('@');

        private System.Guid? userId1 = null;

        private string? userName1 = null;

        private string? displayName1 = null;

        [EmailAddress]
        private string? eMail1 = null;

        private System.DateTime? createdDate1 = null;

        private System.DateTime? lastUpdateDate1 = null;

        public const int MAX_USER_NAME_LENGTH = 32;
        public const int MAX_DISPLAY_NAME_LENGTH = 128;
        public const int MAX_EMAIL_ADDRESS_LENGTH = 128;
    }
    #endregion class Entity


    #region "Repository interface IUserRepository"
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
    #endregion "Repository interface IUserRepository"


    #region "Exception class UserIdIsNotSetException"
    public class UserIdIsNotSetException : System.InvalidOperationException
    {
        public override string Message { get => "User Id must be set first"; }
    }
    #endregion


    #region "Exception class UserNameIsNotSetException"
    public class UserNameIsNotSetException : System.InvalidOperationException
    {
        public override string Message { get => "UserName must be set first"; }
    }
    #endregion


    #region "Exception class UserNameIsEmptyException"
    public class UserNameIsEmptyException : System.ArgumentException
    {
        public override string Message { get => "UserName must not be empty"; }
    }
    #endregion


    #region "Exception class UserNameIsTooLongException"
    public class UserNameIsTooLongException : System.ArgumentException
    {
        public override string Message
        {
            get => "UserName must be " +
                User.MAX_USER_NAME_LENGTH + " characters or shorter";
        }
    }
    #endregion


    #region "Exception class UserNameIsNotValidException"
    public class UserNameIsNotValidException : System.ArgumentException
    {
        public override string Message { get => "UserName must be a legal C# identifier"; }
    }
    #endregion


    #region "Exception class DisplayNameIsNotSetException"
    public class DisplayNameIsNotSetException : System.InvalidOperationException
    {
        public override string Message { get => "DisplayName must be set first"; }
    }
    #endregion


    #region "Exception class DisplayNameIsEmptyException"
    public class DisplayNameIsEmptyException : System.ArgumentException
    {
        public override string Message { get => "DisplayName must not be empty"; }
    }
    #endregion


    #region "Exception class DisplayNameIsTooLongException"
    public class DisplayNameIsTooLongException : System.ArgumentException
    {
        public override string Message
        {
            get => "DisplayName must be " +
                User.MAX_DISPLAY_NAME_LENGTH +
                " characters or shorter";
        }
    }
    #endregion


    #region "Exception class EMailAddressIsNotSetException"
    public class EMailAddressIsNotSetException : System.InvalidOperationException
    {
        public override string Message { get => "User EMail must be set first"; }
    }
    #endregion


    #region "Exception class EMailAddressIsEmptyException"
    public class EMailAddressIsEmptyException : System.ArgumentException
    {
        public override string Message { get => "User EMail must not be empty"; }
    }
    #endregion


    #region "Exception class EMailAddressIsNotValidException"
    public class EMailAddressIsNotValidException : System.ArgumentException
    {
        public override string Message { get => "User EMail must be of the form username@example.com"; }
    }
    #endregion


    #region "Exception class EmailAddressIsTooLongException"
    public class EmailAddressIsTooLongException : System.ArgumentException
    {
        public override string Message
        {
            get => "EmailAddress must be " +
                User.MAX_EMAIL_ADDRESS_LENGTH + " characters or fewer";
        }
    }
    #endregion


    #region "Exception class UserCreatedAtIsNotSetException"
    public class UserCreatedAtIsNotSetException : System.InvalidOperationException
    {
        public override string Message { get => "User CreatedAt must be set first"; }
    }
    #endregion


    #region "Exception class UserUpdatedAtIsNotSetException"
    public class UserUpdatedAtIsNotSetException : System.InvalidOperationException
    {
        public override string Message { get => "User UpdatedAt must be set first"; }
    }
    #endregion
}
#endregion "NotesDomain/UserEntity.cs"
