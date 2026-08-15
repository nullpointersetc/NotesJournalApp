#region "NotesAspFrontEnd.cs"
using Microsoft.AspNetCore.Builder;

#pragma warning disable IDE0130
namespace NullPointersEtc.NotesJournalApp.NotesAspFrontEnd;
public static class NotesAspFrontEnd
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder =
            WebApplication.CreateBuilder(args);

        WebApplication app = builder.Build();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.Run();
    }
}
#endregion "NotesAspFrontEnd.cs"
