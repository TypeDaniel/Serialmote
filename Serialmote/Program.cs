using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RJCP.IO.Ports;
using Serialmote.Controllers;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddIniFile("settings.ini", optional: true, reloadOnChange: true)
    .Build();

// Configure SerialSettings
var serialSettings = new SerialSettings();
configuration.GetSection("SerialSettings").Bind(serialSettings);

builder.Services.AddSingleton(serialSettings); 

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddWindowsService();
builder.Services.AddHostedService<SerialmoteController>();

var app = builder.Build();

//Configure AppSetings
string port = configuration.GetSection("AppSettings")["Port"] ?? "5050";

if (app.Environment.IsProduction())
{
    string url = $"https://localhost:{port}";
    app.Urls.Add(url);
}

app.MapControllers();
app.Run();
