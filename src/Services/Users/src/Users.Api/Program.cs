using DataAccess.Base.Impl;
using DataAccess.Base;
using EventBus.Constants;
using MassTransit;
using System.Reflection;
using Users.Api.Consumers;
using Users.Api.Services;
using Users.Api.Services.Interfaces;
using DataAccess.Persistences;
using Microsoft.EntityFrameworkCore;

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
                new MySqlServerVersion(new Version(8, 0, 23)), mySqlOptions => mySqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null)));

// Add MassTransit
services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(configuration["RabbitMQ:Host"], "/", host =>
        {
            host.Username(configuration["RabbitMQ:Username"]);
            host.Password(configuration["RabbitMQ:Password"]);
        });

        cfg.ReceiveEndpoint(QueuesConsts.UserCreatedEventQueueName + "-user-service", x =>
        {
            x.ConfigureConsumer<UserCreatedEventConsumer>(context);
        });
    });
});

// Add AutoMapper
services.AddAutoMapper(Assembly.GetExecutingAssembly());

services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
