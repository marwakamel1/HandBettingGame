using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HandBettingGame;
using HandBettingGame.Configuration;
using HandBettingGame.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
builder.Services.Configure<GameRulesOptions>(builder.Configuration.GetSection(GameRulesOptions.SectionName));

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<GameStateService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<LeaderboardService>();

await builder.Build().RunAsync();
