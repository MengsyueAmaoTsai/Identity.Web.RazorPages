using Microsoft.AspNetCore.DataProtection;

using RichillCapital.Identity;
using RichillCapital.Identity.Web.Pages;
using RichillCapital.Identity.Web.Middlewares;
using RichillCapital.Identity.Web.Services;
using RichillCapital.Logging;
using RichillCapital.Persistence;
using RichillCapital.UseCases;

using Serilog;

using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Domain layer
// builder.Services.AddUserService();

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
builder.Services.AddMiddlewares();
builder.Services.AddPages();

builder.Services.AddCors(builder =>
{
    builder
        .AddDefaultPolicy(policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
});


// Security
// Get from {"Redis": "connectionString"}
var connectionString = builder.Configuration["Redis"];

builder.Services
    .AddDataProtection()
    .PersistKeysToStackExchangeRedis(
        ConnectionMultiplexer.Connect(connectionString),
        "DataProtection-Keys")
    .SetApplicationName("RichillCapital.Identity.Web");

var app = builder.Build();

app.UseDebuggingRequestMiddleware();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseIdentityServer();
// app.UseAuthentication();
app.UseAuthorization();

app.MapPages();

await app.RunAsync();


public partial class Program;