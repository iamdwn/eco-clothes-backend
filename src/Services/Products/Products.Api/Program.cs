using DataAccess.Base;
using DataAccess.Base.Impl;
using DataAccess.Persistences;
using EventBus.Constants;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orders.Api.Services.Impl;
using Products.Api.Services;
using Products.Api.Services.Impl;

namespace Products.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<EcoClothesContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 23)), 
                mySqlOptions => mySqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null)));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Register Unit of Work
            builder.Services.AddScoped<IProductService, ProductServiceImpl>(); // Register Product Service
            builder.Services.AddScoped<ISizeService, SizeServiceImpl>(); // Register Size Service
            builder.Services.AddScoped<ICategoryService, CategoryServiceImpl>(); // Register Category Service
            builder.Services.AddScoped<StorageServiceImpl>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<StorageServiceImpl>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"], 5672, "/", host =>
                    {
                        host.Username(builder.Configuration["RabbitMQ:Username"]);
                        host.Password(builder.Configuration["RabbitMQ:Password"]);
                    });

                    cfg.ReceiveEndpoint(QueuesConsts.OrderCreated, e =>
                    {
                        e.ConfigureConsumer<StorageServiceImpl>(context);
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
