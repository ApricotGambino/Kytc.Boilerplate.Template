using Api;
using Api.Configurations;
using Microsoft.Extensions.Options;



var builder = WebApplication.CreateBuilder(args);

builder.AddAppSettings();

// Add services to the container.

var app = builder.Build();



// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/appsettings", () =>
{

    var appSettings = app.Services.GetRequiredService<IOptions<AppSettings>>().Value;
    return appSettings;
});

app.Run();

