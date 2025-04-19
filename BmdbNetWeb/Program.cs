using BmdbNetWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BmdbNetWeb {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<BmdbContext>(
               // lambda
               options => options.UseSqlServer(builder.Configuration.GetConnectionString("BmdbConnectionString"))
               );

            builder.Services.AddControllers().AddJsonOptions(opt => {
                opt.JsonSerializerOptions.ReferenceHandler=
                  System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; opt.JsonSerializerOptions.WriteIndented=true;
            });
            // Configure the HTTP request pipeline.
            
            var app = builder.Build();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
