using ComplianceSoftwareWebSite.Components;
using ComplianceSoftwareWebSite.Models.Auth;
using ComplianceSoftwareWebSite.Services;
using ComplianceSoftwareWebSite.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.UseUrls("http://localhost:5003", "https://localhost:5004");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//builder.Services.AddRazorPages();
//builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();//sp => new HttpClient { BaseAddress = new Uri("https://localhost:7216/") });

//builder.Services.AddOptions();
builder.Services.AddHttpContextAccessor(); // Add this for accessing HTTP context

//builder.Services.AddDistributedMemoryCache();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Change to your Redis server configuration
});

// Add session management
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Add authorization policies
builder.Services.AddAuthorization();
//builder.Services.AddTransient<CustomHttpHandlerService>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProviderService>();
builder.Services.AddScoped(s => (AuthStateProviderService)s.GetRequiredService<AuthenticationStateProvider>());
//builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProviderService>();


builder.Services.AddAuthentication();
builder.Services.AddAuthorizationCore();


builder.Services.AddScoped<ILicenseService, LicenseService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();