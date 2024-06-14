using RichillCapital.Identity;
using RichillCapital.Identity.Web.IdentityServer;
using RichillCapital.Identity.Web.Pages;
using RichillCapital.Identity.Web.Services;
using RichillCapital.Logging;
using RichillCapital.Persistence;
using RichillCapital.UseCases;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediator();

// Infrastructure - Logging
builder.WebHost.UseIdentityWebLogger();
builder.Services.AddSerilog();

// Infrastructure - Persistence 
builder.Services.AddDatabase();

// Infrastructure - Identity

// Infrastructure - Services
builder.Services.AddApiService();

builder.Services.AddPages();

builder.Services
    .AddIdentityServer(options =>
    {
        options.UserInteraction.LoginUrl = "/users/signin";
        options.UserInteraction.LoginReturnUrlParameter = "returnUrl";
    })
    .AddInMemoryClients(InMemoryClients.Default)
    .AddInMemoryIdentityResources(InMemoryIdentityResources.Default);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = RichillCapitalAuthenticationSchemes.Cookie;
    })
    .AddCookie(RichillCapitalAuthenticationSchemes.Cookie, options =>
    {
        options.LoginPath = "/users/signin";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapPages();

await app.RunAsync();


public partial class Program;