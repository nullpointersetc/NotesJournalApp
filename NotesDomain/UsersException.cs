#region class Exceptions.UserIdIsNotSetException
#pragma warning disable IDE0130
#pragma warning disable IDE0240
#nullable enable

namespace NullPointersEtc.NotesJournalApp.Exceptions;

public class UserIdIsNotSetException : NotesException
{
    public override string Message { get => "User Id must be set first"; }
}
#endregion class Exceptions.UserIdIsNotSetException

#region class Exceptions.UserIdentifierIsNotSetException
public class UserIdentifierIsNotSetException : NotesException
{
    public override string Message { get => "User Identifier must be set first"; }
}
#endregion class Exceptions.UserIdentifierIsNotSetException

#region class Exceptions.UserIdentifierIsEmptyException
public class UserIdentifierIsEmptyException : NotesException
{
    public override string Message { get => "User Identifier must not be empty"; }
}
#endregion class Exceptions.UserIdentifierIsEmptyException

#region class Exceptions.UserIdentifierIsNotValidException
public class UserIdentifierIsNotValidException : NotesException
{
    public override string Message { get => "User Identifier must be a legal C# identifier"; }
}
#endregion class Exceptions.UserIdentifierIsNotValidException

#region class Exceptions.UserNameIsNotSetException
public class UserNameIsNotSetException : NotesException
{
    public override string Message { get => "User Name must be set first"; }
}
#endregion class Exceptions.UserNameIsNotSetException

#region class Exceptions.UserNameIsEmptyException
public class UserNameIsEmptyException : NotesException
{
    public override string Message { get => "User Name must not be empty"; }
}
#endregion class Exceptions.UserNameIsEmptyException

#region class Exceptions.UserNameIsNotValidException
public class UserNameIsNotValidException : NotesException
{
    public override string Message { get => "User Name must be valid"; }
}
#endregion class Exceptions.UserNameIsNotValidException

#region class Exceptions.UserEMailIsNotSetException
public class UserEMailIsNotSetException : NotesException
{
    public override string Message { get => "User EMail must be set first"; }
}
#endregion class Exceptions.UserEMailIsNotSetException

#region class Exceptions.UserEMailIsEmptyException
public class UserEMailIsEmptyException : NotesException
{
    public override string Message { get => "User EMail must not be empty"; }
}
#endregion class Exceptions.UserEMailIsEmptyException

#region class Exceptions.UserEmailIsNotValidException
public class UserEMailIsNotValidException : NotesException
{
    public override string Message { get => "User EMail must be of the form username@example.com"; }
}
#endregion class Exceptions.UserEmailIsNotValidException

#region class Exceptions.UserCreatedAtIsNotSetException
public class UserCreatedAtIsNotSetException : NotesException
{
    public override string Message { get => "User CreatedAt must be set first"; }
}
#endregion class Exceptions.UserCreatedAtIsNotSetException

#region class Exceptions.UserUpdatedAtIsNotSetException
public class UserUpdatedAtIsNotSetException : NotesException
{
    public override string Message { get => "User UpdatedAt must be set first"; }
}
#endregion class Exceptions.UserUpdatedAtIsNotSetException
