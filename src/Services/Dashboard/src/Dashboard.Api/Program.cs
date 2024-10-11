
using Dashboard.Api.Services;
using Dashboard.Api.Services.Impl;
using DataAccess.Base;
using DataAccess.Base.Impl;
using DataAccess.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Dashboard.Api
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
            builder.Services.AddScoped<IUserAnalytics, UserAnalyticsImpl>();
            builder.Services.AddScoped<IProductAnalytics, ProductAnalyticsImpl>();
            builder.Services.AddScoped<IRevenueAnalytics, RevenueAnalyticsImpl>();
            builder.Services.AddScoped<IOrderAnalytics, OrderAnalyticsImpl>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.EnableAnnotations();
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
