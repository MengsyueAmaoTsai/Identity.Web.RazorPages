using Microsoft.AspNetCore.DataProtection;

using RichillCapital.Identity;
using RichillCapital.Identity.Web.Pages;
using RichillCapital.Identity.Web.Services;
using RichillCapital.Logging;
using RichillCapital.Persistence;
using RichillCapital.UseCases;

using Serilog;

using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Application Layer
builder.Services.AddMediator();

// Infrastructure - Logging
builder.WebHost.UseIdentityWebLogger();
builder.Services.AddSerilog();

// Infrastructure - Persistence 
builder.Services.AddDatabase();

// Infrastructure - Identity
builder.Services.AddIdentityWebIdentity();

// Infrastructure - Services
builder.Services.AddApiService();

// Presentation - RazorPages
builder.Services.AddPages();

// Security
builder.Services
    .AddDataProtection()
    .PersistKeysToStackExchangeRedis(
        ConnectionMultiplexer.Connect("redis://localhost:6379"),
        "DataProtection-Keys")
    .SetApplicationName("RichillCapital.Identity.Web");

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