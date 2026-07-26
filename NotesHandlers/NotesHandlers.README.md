# NotesHandlers project ##################################

The Application layer of the NotesJournalApp solution.

## Arrangement of namespaces and classes and interfaces ##

1.  namespace NullPointersEtc.NotesJournalApp.NoteHandlers
    1.  handler interface INoteHandler
    2.  handler class NoteHandler

2.  namespace NullPointersEtc.NotesJournalApp.UserHandlers
    1.  handler interface IUserHandler
    2.  handler class UserHandler

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
    