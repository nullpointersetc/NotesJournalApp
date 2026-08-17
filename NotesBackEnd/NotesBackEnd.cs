#region "NotesBackEnd/NotesBackEnd.cs"

#pragma warning disable IDE0001, IDE0002, IDE0240
#nullable enable

using NullPointersEtc.NotesJournalApp.NotesHandlers;
using NullPointersEtc.NotesJournalApp.NotesStorage;
using NullPointersEtc.NotesJournalApp.NoteEntity;
using NullPointersEtc.NotesJournalApp.UserEntity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Console = System.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SymmetricSecurityKey = Microsoft.IdentityModel.Tokens.SymmetricSecurityKey;
using Encoding = System.Text.Encoding;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using DateTime = System.DateTime;
using Microsoft.AspNetCore.Http;

#pragma warning disable IDE0130
namespace NullPointersEtc.NotesJournalApp.NotesBackEnd;

public class NotesBackEnd
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder =
            WebApplication.CreateBuilder(args);

        Console.WriteLine(builder.Environment.ApplicationName +
            " is running in environment \"" +
            builder.Environment.EnvironmentName + "\"");

        Console.WriteLine("To change this, set the environment " +
            "variable ASPNETCORE_ENVIRONMENT={your-environment-name}" +
            " before running " + builder.Environment.ApplicationName);

        Console.WriteLine("or use dotnet run --project:NotesBackEnd " +
            "--environment {your-environment-name}");

        string connectionType = builder.Configuration["ConnectionType"]
            ?? throw new System.Exception("ConnectionType not configured.");

        bool bUseSqlServer = connectionType == "SqlServer",
            bUseSqlite = connectionType == "Sqlite";

        if (!bUseSqlServer && !bUseSqlite)
            throw new System.Exception("ConnectionType must be SqlServer or Sqlite");

        string connectionString = builder.Configuration["ConnectionString"]
            ?? throw new System.Exception("ConnectionString not configured.");

        if (string.IsNullOrEmpty(connectionString))
            throw new System.Exception("ConnectionString is empty");

        Console.WriteLine("Connection selected is " + connectionType +
            " with string: " + connectionString);

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("super-secret-key-12345"))
                });

        builder.Services.AddAuthorization();

        builder.Services.AddScoped<INoteHandler, NoteHandler>();
        builder.Services.AddScoped<IUserHandler, UserHandler>();
        builder.Services.AddScoped<INoteRepository, NoteRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        if (bUseSqlServer)
            builder.Services.AddSingleton(
                    MoreOptionsForNotesDbContext.ForSqlServer())
                .AddDbContext<NotesDbContext>(
                    options => options.UseSqlServer(connectionString));

        if (bUseSqlite)
            builder.Services.AddSingleton(
                    MoreOptionsForNotesDbContext.ForSqlite())
                .AddDbContext<NotesDbContext>(
                    options => options.UseSqlite(connectionString));

        const string corsPolicyName = "NotesAspFrontEnd";

        builder.Services.AddCors(
            crossOriginResourceSharingOptions =>
            crossOriginResourceSharingOptions.AddPolicy(
                corsPolicyName,
                policy => policy.WithOrigins("http://localhost:5188")
                    .AllowAnyHeader().AllowAnyMethod()));

        WebApplication app = builder.Build();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors(corsPolicyName);
        NotesRestAPI.MapEndpoints(app);
        UsersRestAPI.MapEndpoints(app);

        app.MapPost("/auth/login", (LoginRequest login) =>
        {
            if (login.userName == "darren" && login.password == "password")
            {
                Claim[] claims = new[]
                {
                    new Claim(ClaimTypes.Name, login.userName)
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("super-secret-key-12345"));

                var creds = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds);

                return Results.Ok(new JsonWebTokenWrapper(
                    token: new JwtSecurityTokenHandler().WriteToken(token)));
            }
            else
            {
                return Results.Unauthorized();
            }
        });

        using (IServiceScope scope = app.Services.CreateScope())
        {
            using NotesDbContext db =
                scope.ServiceProvider.GetRequiredService<NotesDbContext>();

            if (db.Database.IsSqlite())
                db.Database.EnsureCreated();

            if (db.Database.IsSqlServer())
                db.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Run();
    }
}


#pragma warning disable IDE1006, IDE0290
public class LoginRequest
{
    public LoginRequest(string userName, string password)
    {
        myUserName = userName;
        myPassword = password;
    }

    public string userName { get => myUserName; }
    public string password { get => myPassword; }
    private readonly string myUserName;
    private readonly string myPassword;
}


#pragma warning disable IDE1006, IDE0290
public class JsonWebTokenWrapper
{
    public JsonWebTokenWrapper(string token) { myToken = token; }
    public string token { get => myToken; }
    private readonly string myToken;
}

#endregion "NotesBackEnd/NotesBackEnd.cs"
