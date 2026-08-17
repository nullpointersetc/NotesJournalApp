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
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using DateTime = System.DateTime;
using Microsoft.AspNetCore.Http;
using HttpLoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields;
using Microsoft.Extensions.Logging;

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
                    IssuerSigningKey = SuperSecretKey
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
                policy => policy.WithOrigins(
                    "http://localhost:17700", "http://localhost:17701",
                    "http://localhost:17702", "http://localhost:17703",
                    "http://localhost:17704", "http://localhost:17705",
                    "http://localhost:17706", "http://localhost:17707",
                    "http://localhost:17708", "http://localhost:17709",
                    "http://localhost:17710", "http://localhost:17711",
                    "http://localhost:17712", "http://localhost:17713",
                    "http://localhost:17714", "http://localhost:17715",
                    "http://localhost:17716", "http://localhost:17717",
                    "http://localhost:17718", "http://localhost:17719",
                    "https://localhost:17720", "https://localhost:17721",
                    "https://localhost:17722", "https://localhost:17723",
                    "https://localhost:17724", "https://localhost:17725",
                    "https://localhost:17726", "https://localhost:17727",
                    "https://localhost:17728", "https://localhost:17729",
                    "https://localhost:17730", "https://localhost:17731",
                    "https://localhost:17732", "https://localhost:17733",
                    "https://localhost:17734", "https://localhost:17735",
                    "https://localhost:17736", "https://localhost:17737",
                    "https://localhost:17738", "https://localhost:17739")
                    .AllowAnyHeader().AllowAnyMethod()));

        builder.Services.AddHttpLogging(
            logging => logging.LoggingFields =
                HttpLoggingFields.Request |
                HttpLoggingFields.RequestHeaders |
                HttpLoggingFields.Response |
                HttpLoggingFields.ResponseHeaders |
                HttpLoggingFields.Duration);

        WebApplication app = builder.Build();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors(corsPolicyName);
        NotesRestAPI.MapEndpoints(app);
        UsersRestAPI.MapEndpoints(app);

        app.MapPost("/auth/login", (LoginRequest login, ILogger<NotesBackEnd> log) =>
        {
            log.LogInformation("Login attempt for user: {userName}", login.userName);

            if (login.userName == "darren" && login.password == "password")
                return Results.Ok(new JsonWebTokenWrapper(
                    token: new JwtSecurityTokenHandler().WriteToken(
                        new JwtSecurityToken(
                            claims: [new Claim(ClaimTypes.Name, login.userName)],
                            expires: DateTime.UtcNow.AddHours(1),
                            signingCredentials: new SigningCredentials(
                                SuperSecretKey, SecurityAlgorithms.HmacSha256)))));
            else
                return Results.Unauthorized();
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

        app.UseHttpLogging();
        app.Run();
    }


    private static SymmetricSecurityKey SuperSecretKey
    {
        get => new(key: [SUPER_SECRET_KEY_1st, SUPER_SECRET_KEY_2nd,
            SUPER_SECRET_KEY_3rd, SUPER_SECRET_KEY_4th,
            SUPER_SECRET_KEY_5th, SUPER_SECRET_KEY_6th,
            SUPER_SECRET_KEY_7th, SUPER_SECRET_KEY_8th,
            SUPER_SECRET_KEY_9th, SUPER_SECRET_KEY_10th,
            SUPER_SECRET_KEY_11th, SUPER_SECRET_KEY_12th,
            SUPER_SECRET_KEY_13th, SUPER_SECRET_KEY_14th,
            SUPER_SECRET_KEY_15th, SUPER_SECRET_KEY_16th,
            SUPER_SECRET_KEY_17th, SUPER_SECRET_KEY_18th,
            SUPER_SECRET_KEY_19th, SUPER_SECRET_KEY_20th,
            SUPER_SECRET_KEY_21st, SUPER_SECRET_KEY_22nd,
            SUPER_SECRET_KEY_23rd, SUPER_SECRET_KEY_24th,
            SUPER_SECRET_KEY_25th, SUPER_SECRET_KEY_26th,
            SUPER_SECRET_KEY_27th, SUPER_SECRET_KEY_28th,
            SUPER_SECRET_KEY_29th, SUPER_SECRET_KEY_30th,
            SUPER_SECRET_KEY_31st, SUPER_SECRET_KEY_32nd]);
    }

    private const byte SUPER_SECRET_KEY_1st = (byte)230,
        SUPER_SECRET_KEY_2nd = (byte)187,
        SUPER_SECRET_KEY_3rd = (byte)191,
        SUPER_SECRET_KEY_4th = (byte)194,
        SUPER_SECRET_KEY_5th = (byte)183,
        SUPER_SECRET_KEY_6th = (byte)143,
        SUPER_SECRET_KEY_7th = (byte)117,
        SUPER_SECRET_KEY_8th = (byte)73,
        SUPER_SECRET_KEY_9th = (byte)183,
        SUPER_SECRET_KEY_10th = (byte)86,
        SUPER_SECRET_KEY_11th = (byte)171,
        SUPER_SECRET_KEY_12th = (byte)186,
        SUPER_SECRET_KEY_13th = (byte)69,
        SUPER_SECRET_KEY_14th = (byte)2,
        SUPER_SECRET_KEY_15th = (byte)228,
        SUPER_SECRET_KEY_16th = (byte)109,
        SUPER_SECRET_KEY_17th = (byte)18,
        SUPER_SECRET_KEY_18th = (byte)153,
        SUPER_SECRET_KEY_19th = (byte)175,
        SUPER_SECRET_KEY_20th = (byte)172,
        SUPER_SECRET_KEY_21st = (byte)172,
        SUPER_SECRET_KEY_22nd = (byte)233,
        SUPER_SECRET_KEY_23rd = (byte)192,
        SUPER_SECRET_KEY_24th = (byte)70,
        SUPER_SECRET_KEY_25th = (byte)169,
        SUPER_SECRET_KEY_26th = (byte)153,
        SUPER_SECRET_KEY_27th = (byte)28,
        SUPER_SECRET_KEY_28th = (byte)17,
        SUPER_SECRET_KEY_29th = (byte)155,
        SUPER_SECRET_KEY_30th = (byte)167,
        SUPER_SECRET_KEY_31st = (byte)113,
        SUPER_SECRET_KEY_32nd = (byte)231;
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
