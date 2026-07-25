# NotesDomain project ####################################

The Domain layer of the NotesJournalApp solution.

## Arrangement of namespaces and classes and interfaces ##

1.  namespace NullPointersEtc.NotesJournalApp.NoteEntity
    1.  entity class Note
    2.  repository interface INotesRepository
    3.  exception class NoteTitleIsNotSetException
    4.  exception class NoteTitleIsEmptyException
    5.  exception class NoteTitleIsTooLongException
    6.  exception class NoteBodyIsNotSetException
    7.  exception class NoteBodyIsEmptyException
    8.  exception class NoteBodyIsTooLongException
    9.  exception class NoteCreationDateIsNotSetException
    10. exception class NoteLastModifiedDateIsNotSetException

2.  namespace NullPointersEtc.NotesJournalApp.UserEntity
    1.  entity class User
    2.  repository interface IUsersRepository
    3.  exception class UserIdIsNotSetException
    4.  exception class UserNameIsNotSetException
    5.  exception class UserNameIsEmptyException
    6.  exception class UserNameIsTooLongException
    7.  exception class UserNameIsNotValidException
    8.  exception class DisplayNameIsNotSetException
    9.  exception class DisplayNameIsEmptyException
    10. exception class DisplayNameIsTooLongException
    11. exception class EMailAddressIsNotSetException
    12. exception class EMailAddressIsEmptyException
    12. exception class EMailAddressIsNotValidException
    13. exception class EMailAddressIsTooLongException
    14. exception class UserCreationDateIsNotSetException
    15. exception class UserLastModifiedDateIsNotSetException

## Business rules ########################################

1.  A Note must contain a title and a body.
2.  A Note has a GUID that is used as the primary key.
3.  A Note's title is not unique.
4.  Future direction: a note must be associated with
    the user who created it.
5.  Future direction: a note's title is case-insensitive.
6.  A Note keeps track of when it was created
    and when it was last modified.
7.  Future direction: a note can be deleted.
8.  A User must contain a username, a display name,
    and an e-mail address.
9.  A User's username must have the form of a C#
    identifier.
10. An e-mail address must be in the proper format.
11. A User keeps track of when it was created
    and when it was last modified.
12. A User's username cannot be changed after
    the User is created.
13. Future direction: A User cannot actually be deleted;
    a user can be made active or inactive.
    