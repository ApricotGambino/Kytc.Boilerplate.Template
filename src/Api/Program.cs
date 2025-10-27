using Api;
using Api.Configurations;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;



var builder = WebApplication.CreateBuilder(args);

builder.AddAppSettings();

// Add services to the container.

var app = builder.Build();
//builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template;Trusted_Connection=True;MultipleActiveResultSets=true");

    options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
});

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Kytc.Boilerplate.Template;Trusted_Connection=True;MultipleActiveResultSets=true",
//                    opt => opt.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));



// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/appsettings", () =>
{
    var appSettings = app.Services.GetRequiredService<IOptions<AppSettings>>().Value;
    return appSettings;
});

app.Run();

