
using DataAccess.Base;
using DataAccess.Base.Impl;
using DataAccess.Persistences;
using EventBus.Constants;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Api.Services;
using Orders.Api.Services.Impl;

namespace Orders.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<EcoClothesContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 23))));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IMassTransitService, MassTransitServiceImpl>();
            builder.Services.AddScoped<IOrderService, OrderServiceImpl>();
            builder.Services.AddScoped<IOrderItemService, OrderItemServiceImpl>();
            builder.Services.AddScoped<OrderApprovalServiceImpl>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderApprovalServiceImpl>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"], 5672, "/", host =>
                    {
                        host.Username(builder.Configuration["RabbitMQ:Username"]);
                        host.Password(builder.Configuration["RabbitMQ:Password"]);
                    });

                    cfg.ReceiveEndpoint(QueuesConsts.PaymentApproval, e =>
                    {
                        e.ConfigureConsumer<OrderApprovalServiceImpl>(context);
                    });
                });
            });

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
        }
    }
}
