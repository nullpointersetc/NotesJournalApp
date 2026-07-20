#region class Entities.User
#pragma warning disable IDE0240
#nullable enable

using UserIdIsNotSetException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserIdIsNotSetException;

using UserIdentifierIsNotSetException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserIdentifierIsNotSetException;

using UserIdentifierIsEmptyException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserIdentifierIsEmptyException;

using UserIdentifierIsNotValidException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserIdentifierIsNotValidException;

using UserNameIsNotSetException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserNameIsNotSetException;

using UserNameIsEmptyException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserNameIsEmptyException;

using UserNameIsNotValidException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserNameIsNotValidException;

using UserEMailIsNotSetException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserEMailIsNotSetException;

using UserEMailIsEmptyException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserEMailIsEmptyException;

using UserEMailIsNotValidException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserEMailIsNotValidException;

using UserCreatedAtIsNotSetException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserCreatedAtIsNotSetException;

using UserUpdatedAtIsNotSetException =
    NullPointersEtc.NotesJournalApp.Exceptions.UserUpdatedAtIsNotSetException;

#pragma warning disable IDE0130
namespace NullPointersEtc.NotesJournalApp.Entities;

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
    private string? myEMail = null;
    private System.DateTime? myCreatedAt = null;
    private System.DateTime? myUpdatedAt = null;
}



#endregion class Entities.User
