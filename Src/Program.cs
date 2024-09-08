using Microsoft.AspNetCore.DataProtection;
using RichillCapital.Infrastructure.Identity;
using RichillCapital.Infrastructure.Identity.Server;
using RichillCapital.Infrastructure.Logging;
using RichillCapital.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure layer - Logging
builder.WebHost.UseCustomLogger();
builder.Services.AddSerilog();

// Infrastructure layer - Identity
builder.Services.AddCustomIdentity();
builder.Services.AddIdentityServerServices();

// Infrastructure layer - Persistence
builder.Services.AddDatabase();

// Presentation layer
builder.Services
    .AddRazorPages()
    .WithRazorPagesRoot("/Src/Pages");

builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(builder.Environment.ContentRootPath));

var app = builder.Build();

app.ResetDatabase();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();

await app.RunAsync();
