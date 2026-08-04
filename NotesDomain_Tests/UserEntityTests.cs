﻿#region "UserEntityTests.cs"
#pragma warning disable IDE0130

namespace NullPointersEtc.NotesJournalApp.NotesDomain_Tests
{
    using User = NullPointersEtc.NotesJournalApp.UserEntity.User;

    using UserIdIsNotSetException =
        NullPointersEtc.NotesJournalApp.UserEntity.UserIdIsNotSetException;

    using UserNameIsNotSetException =
        NullPointersEtc.NotesJournalApp.UserEntity.UserNameIsNotSetException;

    using UserNameIsEmptyException =
        NullPointersEtc.NotesJournalApp.UserEntity.UserNameIsEmptyException;

    using UserNameIsTooLongException =
        NullPointersEtc.NotesJournalApp.UserEntity.UserNameIsTooLongException;

    using UserNameIsNotValidException =
        NullPointersEtc.NotesJournalApp.UserEntity.UserNameIsNotValidException;

    using NoteBodyIsNotSetException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteBodyIsNotSetException;

    using NoteBodyIsEmptyException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteBodyIsEmptyException;

    using NoteBodyIsTooLongException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteBodyIsTooLongException;

    using NoteLastModifiedDateIsNotSetException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteLastModifiedDateIsNotSetException;

    using NoteLastModifiedDateCannotGoBackInTimeException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteLastModifiedDateCannotGoBackInTimeException;

    using NoteCreationDateIsNotSetException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteCreationDateIsNotSetException;

    using NoteCreationDateIsNotModifiableException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteCreationDateIsNotModifiableException;

    using Assert = Xunit.Assert;
    using Fact = Xunit.FactAttribute;

    public class UserEntityTests
    {
        [@Fact]
        public void CannotGetUserIdBeforeSet()
        {
            User user = new();
            Assert.Throws<UserIdIsNotSetException>(() => user.UserID);
        }

        [@Fact]
        public void CannotGetUserNameBeforeSet()
        {
            User user = new();
            Assert.Throws<UserNameIsNotSetException>(() => user.UserName);
        }

        [@Fact]
        public void CannotSetUserNameToNull()
        {
            User user = new();

            Assert.Throws<UserNameIsEmptyException>(
                () => user.UserName = null!);
        }

        [@Fact]
        public void CannotSetUserNameToEmptyString()
        {
            User user = new();

            Assert.Throws<UserNameIsEmptyException>(
                () => user.UserName = "");
        }

        [@Fact]
        public void CannotSetUserNameToOnlyWhitespace()
        {
            string invalidUserName = " ";

            User user = new();

            Assert.Throws<UserNameIsEmptyException>(
                () => user.UserName = invalidUserName);
        }

        [@Fact]
        public void CanOnlySetUserNameToAnIdentifier()
        {
            string invalidUserName = "Not an identifier";

            Assert.False(User.IdentifierIsValid(invalidUserName),
                userMessage: nameof(invalidUserName) +
                    " must actually be invalid to test the exception.");

            User user = new();

            Assert.Throws<UserNameIsNotValidException>(
                () => user.UserName = invalidUserName);
        }

        [@Fact]
        public void CannotSetUserNameToIdentifierThatIsTooLong()
        {
            string invalidUserName = "This_is_a_33_character_identifier";

            Assert.Equal(expected: User.MAX_USER_NAME_LENGTH + 1,
                actual: invalidUserName.Length);

            User user = new();

            Assert.Throws<UserNameIsTooLongException>(
                () => user.UserName = invalidUserName);
        }

        [@Fact]
        public void CanSetUserNameToIdentifierThatIsMaximumLength()
        {
            string validUserName = "The_identifier_has_32_characters";

            Assert.Equal(expected: User.MAX_USER_NAME_LENGTH,
                            actual: validUserName.Length);

            Assert.True(User.IdentifierIsValid(validUserName));

            User user = new();

            user.UserName = validUserName;

            Assert.Equal(expected: validUserName,
                actual: user.UserName);
        }
    }
}

#endregion "UserEntityTests.cs"
