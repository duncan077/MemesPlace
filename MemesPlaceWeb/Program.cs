using Blazored.LocalStorage;
using MemesPlaceWeb;
using MemesPlaceWeb.Providers;
using MemesPlaceWeb.Services.Auth;
using MemesPlaceWeb.Services.Base;
using MemesPlaceWeb.Services.Disqus;
using MemesPlaceWeb.Services.Meme;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://memesplace-production.up.railway.app") });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ApiAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p=>p.GetRequiredService<ApiAuthStateProvider>());
builder.Services.AddScoped<DisqusState>();
builder.Services.AddScoped<IClient, Client>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IMemesService, MemesService>();
await builder.Build().RunAsync();
