using RichillCapital.Identity.Web.Pages;
using RichillCapital.Logging;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure - Logging
builder.WebHost.UseIdentityWebLogger();
builder.Services.AddSerilog();

// Infrastructure - Identity

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

app.UseAuthorization();

app.MapPages();

await app.RunAsync();

public partial class Program;