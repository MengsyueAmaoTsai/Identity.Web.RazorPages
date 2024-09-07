using RichillCapital.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure layer - Persistence
builder.Services.AddDatabase();

// Persentation layer
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
