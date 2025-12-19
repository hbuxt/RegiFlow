using Api.Infrastructure.Cors;
using Api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
        
            builder.Host.UseSerilog();
            builder.Services.AddControllers();
            builder.Services.AddInfrastructure(builder.Configuration);

            var app = builder.Build();
        
            app.UseCors(CorsPolicies.RegiFlowClient);
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}