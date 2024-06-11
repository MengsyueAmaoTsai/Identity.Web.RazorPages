using RichillCapital.Persistence;
using RichillCapital.UseCases;
using RichillCapital.Identity.Web.IdentityServer;
using RichillCapital.Identity.Web.Pages;
using RichillCapital.Identity.Web.Services;
using RichillCapital.Logging;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediator();

// Infrastructure - Logging
builder.WebHost.UseIdentityWebLogger();
builder.Services.AddSerilog();

// Infrastructure - Persistence 
builder.Services.AddDatabase();

// Infrastructure - Identity
builder.Services.ConfigureIdentityServer();

// Infrastructure - Services
builder.Services.AddApiService();

builder.Services.AddPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapPages();

await app.RunAsync();


public partial class Program;