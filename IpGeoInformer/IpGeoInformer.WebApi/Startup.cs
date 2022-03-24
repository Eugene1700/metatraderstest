using IpGeoInformer.FileStorageDal;
using IpGeoInformer.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;

namespace IpGeoInformer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddMemoryCache();
            services.AddServices();
            services.AddHostedService<DataLoaderService>();
            
            services.AddOpenApiDocument(settings =>
            {
                settings.Title = "IpGeoInformer.WebApi";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                });
            }

            app.UseRouting();
            app.UseMiddleware<IpGeoInformerExceptionMiddleware>();
            app.UseSwaggerUi3();
            app.UseOpenApi(configure =>
            {
                configure.PostProcess = (document, _) => document.Schemes = new[]
                {
                    OpenApiSchema.Https,
                    OpenApiSchema.Http
                };
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}