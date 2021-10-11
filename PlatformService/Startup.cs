using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PlatformService.Data;
using PlatformService.SyncDataService.Http;
using System;

namespace PlatformService
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this._env = env;
        }

        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _env;

        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                Console.WriteLine("--> Using SQL Database.");
                services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("PlatformsCon")));
            }
            else
            {
                Console.WriteLine("--> Using InMemory Database.");
                services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemory"));
            }

            services.AddScoped<IPlatformRepo, PlatformRepo>();
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
            });

            Console.WriteLine($"--> CommandServiceBaseUrl: {Configuration["CommandServiceBaseUrl"]}");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));


            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDb.PrePopulation(app, env.IsProduction());
        }
    }
}
