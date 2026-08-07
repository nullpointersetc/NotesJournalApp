﻿#region "NotesDomain_Tests/UserEntityTests.cs"
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

    using EMailAddressIsNotSetException =
        NullPointersEtc.NotesJournalApp.UserEntity.EMailAddressIsNotSetException;

    using EmailAddressIsTooLongException =
        NullPointersEtc.NotesJournalApp.UserEntity.EmailAddressIsTooLongException;

    using UserCreatedAtIsNotSetException=
        NullPointersEtc.NotesJournalApp.UserEntity.UserCreatedAtIsNotSetException;

    using UserCreatedAtWouldBeChangedException=
        NullPointersEtc.NotesJournalApp.UserEntity.UserCreatedAtWouldBeChangedException;

    using UserLastModifiedAtIsNotSetException=
        NullPointersEtc.NotesJournalApp.UserEntity.UserLastModifiedAtIsNotSetException;

    using UserLastModifiedDateWouldGoBackInTimeException =
        NullPointersEtc.NotesJournalApp.UserEntity.UserLastModifiedDateWouldGoBackInTimeException;

    using Assert = Xunit.Assert;
    using Fact = Xunit.FactAttribute;
    using NullPointersEtc.NotesJournalApp.UserEntity;

    public class UserEntityTests
    {
        [method: @Fact]
        public void CannotGetUserIdBeforeSet()
        {
            User user = new();
            Assert.Throws<UserIdIsNotSetException>(() => user.UserID);
        }


        [method: @Fact]
        public void CannotGetUserNameBeforeSet()
        {
            User user = new();
            Assert.Throws<UserNameIsNotSetException>(() => user.UserName);
        }

        [method: @Fact]
        public void CannotSetUserNameToNull()
        {
            User user = new();

            Assert.Throws<UserNameIsEmptyException>(
                () => user.UserName = null!);
        }


        [method: @Fact]
        public void CannotSetUserNameToEmptyString()
        {
            User user = new();

            Assert.Throws<UserNameIsEmptyException>(
                () => user.UserName = string.Empty);
        }


        [method: @Fact]
        public void CannotSetUserNameToOnlyWhitespace()
        {
            string invalidUserName = " ";

            User user = new();

            Assert.Throws<UserNameIsEmptyException>(
                () => user.UserName = invalidUserName);
        }


        [method: @Fact]
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


        [method: @Fact]
        public void CannotSetUserNameToIdentifierThatIsTooLong()
        {
            string invalidUserName = "This_is_a_33_character_identifier";

            Assert.Equal(expected: User.MAX_USER_NAME_LENGTH + 1,
                actual: invalidUserName.Length);

            User user = new();

            Assert.Throws<UserNameIsTooLongException>(
                () => user.UserName = invalidUserName);
        }


        [method: @Fact]
        public void CanSetUserNameToIdentifierThatIsMaximumLength()
        {
            string validUserName = "The_identifier_has_32_characters";

            Assert.Equal(expected: User.MAX_USER_NAME_LENGTH,
                            actual: validUserName.Length);

            Assert.True(User.IdentifierIsValid(validUserName));

            User user = new();
            {
                user.UserName = validUserName;
            }

            Assert.Equal(expected: validUserName,
                actual: user.UserName);
        }

        [method: @Fact]
        public void CannotGetEmailAddressBeforeSet()
        {
            User user = new();

            Assert.Throws<EMailAddressIsNotSetException>(
                () => user.EMailAddress);
        }

        [method: @Fact]
        public void CanGetEmailAddressAfterSet()
        {
            string sherlockHolmesAtExampleDotNet =
                "sherlockHolmes@example.net";

            Assert.True(User.EMailIsValid(
                sherlockHolmesAtExampleDotNet));

            User user = new();
            {
                user.EMailAddress = sherlockHolmesAtExampleDotNet;
            }

            Assert.Equal(expected: sherlockHolmesAtExampleDotNet,
                actual: user.EMailAddress);
        }

        [@Fact]
        public void CannotSetEmailAddressThatIsTooLong()
        {
            string invalidEMailAddress =
                "Sherlock_Holmes_and_Doctor_Watson_" +
                "and_James_Mortimer_MRCS_"+
                "and_Sir_Henry_Baskerville_" +
                "and_Inspector_Lestrade123"+
                "@grimpen.example.net";

            Assert.Equal(expected: User.MAX_EMAIL_ADDRESS_LENGTH + 1,
                actual: invalidEMailAddress.Length);

            User user = new();

            Assert.Throws<EmailAddressIsTooLongException>(
                () => user.EMailAddress = invalidEMailAddress);
        }

        [@Fact]
        public void CannotSetEmailAddressWithoutCommercialAt()
        {
            string invalidEMailAddress =
                "Sherlock_Holmes_and_Doctor_Watson_" +
                "and_James_Mortimer_MRCS_"+
                "and_Sir_Henry_Baskerville_" +
                "and_Inspector_Lestrade12"+
                "_grimpen.example.net";

            Assert.Equal(expected: User.MAX_EMAIL_ADDRESS_LENGTH,
                actual: invalidEMailAddress.Length);

            Assert.False(User.EMailIsValid(invalidEMailAddress));

            User user = new();

            Assert.Throws<EMailAddressIsNotValidException>(
                () => user.EMailAddress = invalidEMailAddress);
        }

       [method: @Fact]
        public void CannotGetCreatedAtBeforeSet()
        {
            User user = new();

            Assert.Throws<UserCreatedAtIsNotSetException>(() => user.CreatedAt);
        }

        [method: @Fact]
        public void CanGetCreatedAtAfterSet()
        {
            System.DateTime testDate = new(
                year: 2026, month: 7, day: 1,
                hour: 12, minute: 34, second: 56,
                millisecond: 789);

            User user = new();
            {
                user.CreatedAt = testDate;

                Assert.Equal(expected: testDate,
                    actual: user.CreatedAt);
            }
        }

        [method: @Fact]
        public void CanSetCreatedAtToSameDateTime()
        {
            System.DateTime testDate1 = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            System.DateTime testDate2 = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            User user = new();
            {
                user.CreatedAt = testDate1;
            }

            Assert.Equal(expected: testDate1,
                actual: user.CreatedAt);

            user.CreatedAt = testDate2;

            Assert.Equal(expected: testDate2,
                actual: user.CreatedAt);
        }

        [method: @Fact]
        public void CannotSetCreatedAtToDifferentDateTime()
        {
            System.DateTime testDate1 = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            System.DateTime testDate2 = new(
                year: 2026, month: 7, day: 1,
                hour: 15, minute: 34, second: 56,
                millisecond: 789);

            User user = new();
            {
                user.CreatedAt = testDate1;
            }

            Assert.Equal(expected: testDate1,
                actual: user.CreatedAt);

            Assert.Throws<UserCreatedAtWouldBeChangedException>(
                () => user.CreatedAt = testDate2);

            Assert.Equal(expected: testDate1,
                actual: user.CreatedAt);
        }

        [method: @Fact]
        public void CannotGetLastModifiedAtBeforeSet()
        {
            User user = new();

            Assert.Throws<UserLastModifiedAtIsNotSetException>(
                () => user.LastModifiedAt);
        }

        [method: @Fact]
        public void CanGetLastModifiedAtAfterSet()
        {
            System.DateTime testDate = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            User user = new();
            {
                user.LastModifiedAt = testDate;

                Assert.Equal(expected: testDate,
                    actual: user.LastModifiedAt);
            }
        }

        [method: @Fact]
        public void CanSetLastModifiedAtToSameDateTime()
        {
            System.DateTime testDate1 = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            System.DateTime testDate2 = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            User user = new();
            {
                user.LastModifiedAt = testDate1;
            }

            Assert.Equal(expected: testDate1,
                actual: user.LastModifiedAt);

            user.LastModifiedAt = testDate2;

            Assert.Equal(expected: testDate2,
                actual: user.LastModifiedAt);
        }

        [method: @Fact]
        public void CanSetLastModifiedAtToLaterDateTime()
        {
            System.DateTime testDate1 = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            System.DateTime testDate2 = new(
                year: 2026, month: 7, day: 1,
                hour: 15, minute: 34, second: 56,
                millisecond: 789);

            User user = new();
            {
                user.LastModifiedAt = testDate1;
            }

            Assert.Equal(expected: testDate1,
                actual: user.LastModifiedAt);

            user.LastModifiedAt = testDate2;

            Assert.Equal(expected: testDate2,
                actual: user.LastModifiedAt);
        }

        [method: @Fact]
        public void CannotSetUpdatedAtToEarlierDateTime()
        {
            System.DateTime testDate1 = new(
                year: 2026, month: 7, day: 1,
                hour: 13, minute: 34, second: 56,
                millisecond: 789);

            System.DateTime testDate2 = new(
                year: 2026, month: 7, day: 1,
                hour: 11, minute: 34, second: 56,
                millisecond: 789);

            User user = new();
            {
                user.LastModifiedAt = testDate1;
            }

            Assert.Equal(expected: testDate1,
                actual: user.LastModifiedAt);

            Assert.Throws<UserLastModifiedDateWouldGoBackInTimeException>(
                () => user.LastModifiedAt = testDate2);

            Assert.Equal(expected: testDate1,
                actual: user.LastModifiedAt);
        }
    }
}

#endregion "NotesDomain_Tests/UserEntityTests.cs"
