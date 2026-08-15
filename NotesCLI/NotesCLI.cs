#region "NotesCLI/NotesCLI.cs"

using Console = System.Console;
using Uri = System.Uri;
using System.Net.Http;
using System.Net.Http.Json;
using Task = System.Threading.Tasks.Task;

#pragma warning disable IDE0130
namespace NullPointersEtc.NotesJournalApp.NotesCLI;

public class NotesCLI
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
            WriteUsage();
        else if (args[0].StartsWith("http:"))
            Run(args[0], args, 1);
        else if (args[0].StartsWith("https:"))
            Run(args[0], args, 1);
        else
            Run(DefaultRestApiURI(), args, 0);
    }


    private static void WriteUsage()
    {
        Console.Error.WriteLine(
            "Usage: .\\NotesCLI.exe [host] CreateUser <username> <displayname> <email>");

        Console.Error.WriteLine(
            "[host]       if specified, must be a URI beginning with http: or https:");

        string indent = "             ";
        Console.Error.WriteLine(indent + "that is running the REST API.");
        Console.Error.WriteLine(indent + "if not specified, the REST API is assumed to be running on");
        Console.Error.WriteLine(indent + DefaultRestApiURI());

        Console.Error.WriteLine(
            "CreateHost   is the only test supported now.");

        Console.Error.WriteLine(
            "<username>   is the username (a C# identifier) to be used.");

        Console.Error.WriteLine(
            "<display>    is the displayable name to be used.");

        Console.Error.WriteLine(
            "<email>      is the e-mail address to be used.");
    }


    private static void Run(
        string uri, string[] args, int startarg)
    {
        if (args[startarg] == "CreateUser"
            && args.Length == startarg + 4)
            CreateUseAsync(uri: uri,
                userName: args[startarg + 1],
                displayName: args[startarg + 2],
                eMailAddress: args[startarg + 3]).Wait();
        else
            WriteUsage();
    }


    private static string DefaultRestApiURI() => "http://localhost:5120";


    private static async Task CreateUseAsync(
        string uri,
        string userName,
        string displayName,
        string eMailAddress)
    {
        HttpClient client = new()
        {
            BaseAddress = new Uri(uri)
        };

        CreateUserDTO newUser = new(
            username: userName,
            displayname: displayName,
            emailaddress: eMailAddress);

        try
        {
            HttpResponseMessage response =
                await client.PostAsJsonAsync(
                    "/api/users", newUser);

            Console.Write("Status: ");
            Console.WriteLine(response.StatusCode);

            string body =
                await response.Content.ReadAsStringAsync();

            Console.WriteLine("Response body:");
            Console.WriteLine(body);
        }
        catch (System.Exception caught)
        {
            Console.WriteLine("Exception:");
            Console.WriteLine(caught);
        }
    }

}


internal class CreateUserDTO
{
    public CreateUserDTO(string username,
        string displayname, string emailaddress)
    {
        userName = username;
        displayName = displayname;
        eMailAddress = emailaddress;
    }

    public string UserName { get => userName; }
    public string DisplayName { get => displayName; }
    public string EMailAddress { get => eMailAddress; }

    private string userName, displayName, eMailAddress;
}

#endregion "NotesCLI/NotesCLI.cs"
