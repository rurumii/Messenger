using Microsoft.EntityFrameworkCore;
using MessageService.Data;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MessageService.Mapping;
using MessageService.Services;

namespace MessageService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<MessageDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MessageProfile));
            builder.Services.AddHttpClient<UserServiceClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5156/");
            });

            var app = builder.Build();

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
