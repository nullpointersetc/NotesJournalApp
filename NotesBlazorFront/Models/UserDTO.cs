#region "UserDTO.cs"
namespace NullPointersEtc.NotesJournalApp.NotesBlazorFront.Models;
public record UserDTO(
    System.Guid UserID,
    string Identifier,
    string Display,
    string EMail,
    System.DateTime CreatedAt,
    System.DateTime UpdatedAt);
#endregion "UserDTO.cs"
