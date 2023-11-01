
using Microsoft.EntityFrameworkCore;
using Protivity.Assignment.CustomerApi.BusinessLogic;
using Protivity.Assignment.CustomerApi.BusinessLogic.Interface;
using Protivity.Assignment.CustomerApi.Common;
using Protivity.Assignment.CustomerApi.Common.Helpers;
using Protivity.Assignment.CustomerApi.Common.Middleware;
using Protivity.Assignment.CustomerApi.Filters;
using Protivity.Assignment.CustomerApi.Repository;
using Protivity.Assignment.CustomerApi.Repository.DataModel;

namespace Protivity.Assignment.CustomerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add configuration settings as needed
            builder.Configuration.AddJsonFile("appsettings.json");
            var connectionString = builder.Configuration.GetConnectionString("MainConnectionString");
            // registed Context with the DI Container
            builder.Services.AddDbContext<CustomerContext>(options =>
                options.UseSqlite(connectionString));
            // Add services to the container.
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IMapper, Mapper>();
            builder.Services.AddScoped<ICustomerBusinessLogic, CustomerBusinessLogic>();
            
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<ModelExceptionFilter>();
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            // use Exception Middleware
            app.UseMiddleware<ExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseRouting();
            app.Run();
        }
    }
}