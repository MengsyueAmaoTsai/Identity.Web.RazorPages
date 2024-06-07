using RichillCapital.Identity.Web.Pages;

var builder = WebApplication.CreateBuilder(args);

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