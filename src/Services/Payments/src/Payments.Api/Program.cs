using DataAccess.Base.Impl;
using DataAccess.Base;
using DataAccess.Persistences;
using Microsoft.EntityFrameworkCore;
using Payments.Api.Services;
using Payments.Api.Services.Interfaces;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// Add DbContext
services.AddDbContext<EcoClothesContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 23)),
    mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5, // Number of retry attempts
                    maxRetryDelay: TimeSpan.FromSeconds(30), // Maximum delay between retries
                    errorNumbersToAdd: null // Add specific error codes if necessary
                )));

// Add PaymentService
services.AddScoped<IPaymentService, PaymentService>();

// Add UnitOfWork
services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add MassTransit
services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], 5672, "/", host =>
        {
            host.Username(builder.Configuration["RabbitMQ:Username"]);
            host.Password(builder.Configuration["RabbitMQ:Password"]);
        });
    });
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

services.AddScoped<IMassTransitService, MassTransitService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
