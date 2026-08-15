#region "NotesAspFrontEnd.cs"
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;
namespace NullPointersEtc.NotesJournalApp.NotesAspFrontEnd;
public static class NotesAspFrontEnd
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}
#endregion "NotesAspFrontEnd.cs"
