#region "CreateUserDTO.cs"
namespace NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models;
public record CreateUserDTO(
    string Identifier,
    string Display, string EMail);
#endregion "CreateUserDTO.cs"
