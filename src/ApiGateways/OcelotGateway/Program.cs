using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");

var services = builder.Services;

// Add CORS policy to allow requests from other origins
services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
    });
});

// Config Ocelot
services.AddOcelot();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

await app.UseOcelot();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
