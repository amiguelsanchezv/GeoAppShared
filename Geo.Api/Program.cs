
using Geo.Application;
using Geo.Service;
using Geo.Service.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Geo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            var geoDB = configuration.GetConnectionString("GeoDB") ?? "";
            var geoDBName = configuration.GetConnectionString("GeoDBName") ?? "";
            var geoCollectionName = configuration.GetConnectionString("GeoCollectionName") ?? "";
            var ofscClientId = configuration.GetConnectionString("OFSCClientID") ?? "";
            var ofscClientSecret = configuration.GetConnectionString("OFSCClientSecret") ?? "";
            var ofscInstance = configuration.GetConnectionString("OFSCInstance") ?? "";
            var createGeoreferencePoint = typeof(CreateGeoreferencePoint).Assembly;
            var updateGeoreferencePoint = typeof(UpdateGeoreferencePointOFSC).Assembly;
            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            builder.Services.AddApplication();
            builder.Services.AddService();
            builder.Services.AddScoped<GeoreferencePointAccess>(x => new GeoreferencePointAccess(geoDB, geoDBName, geoCollectionName));
            builder.Services.AddScoped<GeoreferencePointOFSCContext>(x => new GeoreferencePointOFSCContext(ofscInstance, ofscClientId, ofscClientSecret));
            builder.Services.AddScoped<IOperations, Operations>();
            builder.Services.AddScoped<IGeoreferencePointRepository, GeoreferencePointRepository>();
            builder.Services.AddDbContext<GeoreferencePointDbContext>(o => o.UseMongoDB(geoDB, geoDBName));
            builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(createGeoreferencePoint));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Geo.Api", Version = "v1" });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Geo.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.MapControllers();

            app.Run();
        }
    }
}
