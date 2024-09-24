using EventBus.Constants;
using MassTransit;
using Notification.Consumers;
using Notification.Services;
using Notification.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreatedEventConsumer>();
    x.AddConsumer<UserPasswordResetOccurredEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(configuration["RabbitMQ:Host"], "/", host =>
        {
            host.Username(configuration["RabbitMQ:Username"]);
            host.Password(configuration["RabbitMQ:Password"]);
        });

        cfg.ReceiveEndpoint(QueuesConsts.UserCreatedEventQueueName, x =>
        {
            x.ConfigureConsumer<UserCreatedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint(QueuesConsts.UserPasswordResetOccurredQueueName, x =>
        {
            x.ConfigureConsumer<UserPasswordResetOccurredEventConsumer>(context);
        });
    });
});

services.AddTransient<IEmailSender, MessageServices>();
services.AddTransient<MessageServices>();

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
