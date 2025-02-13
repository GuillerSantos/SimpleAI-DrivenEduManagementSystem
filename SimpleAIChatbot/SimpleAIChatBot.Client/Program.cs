using SimpleAIChatbot.Client.Components;
using MudBlazor.Services;
using SimpleAIChatbot.Client.Services.Contracts;
using SimpleAIChatbot.Client.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7274") });

builder.Services.AddScoped<IChatbotService ,ChatbotService>();

builder.Services.AddMudServices();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();