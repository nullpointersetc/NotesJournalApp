using NullPointersEtc.NotesJournalApp.NotesBlazorFront.Components;

namespace NullPointersEtc.NotesJournalApp.NotesBlazorFront
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddScoped(sp =>
            {
                string NotesBackEndURL = "https://localhost:7023";

                HttpClient client = new()
                {
                    BaseAddress = new Uri(NotesBackEndURL)
                };
                return client;
            });

            builder.Services.AddScoped<
                NullPointersEtc.NotesJournalApp.NotesBlazorFront.Services.NotesApiClient>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
