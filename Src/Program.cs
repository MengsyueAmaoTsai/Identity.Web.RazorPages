using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using RichillCapital.Identity.Web.Middlewares;
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

builder.Services.AddMiddlewares();

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

builder.Services
    .Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });

builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(builder.Environment.ContentRootPath))
    .SetApplicationName("RichillCapital");

var app = builder.Build();

app.UseForwardedHeaders();

app.UseRequestDebuggingMiddleware();

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
app.UseAuthorization();

app.MapRazorPages();

await app.RunAsync();
