using Serialmote.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddWindowsService();
builder.Services.AddHostedService<SerialmoteController>();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    // Use port 5050 for the Production environment
    app.Urls.Add("https://localhost:5050");
}

app.MapControllers();

app.Run();
