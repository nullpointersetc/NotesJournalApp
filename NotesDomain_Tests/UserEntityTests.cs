﻿#region "UserEntityTests.cs"
#pragma warning disable IDE0130

namespace NullPointersEtc.NotesJournalApp.NotesDomain_Tests
{
    using User = NullPointersEtc.NotesJournalApp.UserEntity.User;

    using UserIdIsNotSetException =
        NullPointersEtc.NotesJournalApp.UserEntity.UserIdIsNotSetException;

    using NoteIdIsNotSetException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteIdIsNotSetException;

    using NoteTitleIsNotSetException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteTitleIsNotSetException;

    using NoteTitleIsEmptyException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteTitleIsEmptyException;

    using NoteTitleIsTooLongException =
        NullPointersEtc.NotesJournalApp.NoteEntity.NoteTitleIsTooLongException;

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
    }
}

#endregion "UserEntityTests.cs"
