using RichillCapital.Infrastructure.Identity;
using RichillCapital.Infrastructure.Logging;
using RichillCapital.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure layer - Logging
builder.WebHost.UseCustomLogger();
builder.Services.AddSerilog();

// Infrastructure layer - Identity
builder.Services.AddCustomIdentity();

// Infrastructure layer - Persistence
builder.Services.AddDatabase();

// Presentation layer
builder.Services
    .AddRazorPages()
    .WithRazorPagesRoot("/Src/Pages");

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

app.UseAuthorization();

app.MapRazorPages();

await app.RunAsync();
